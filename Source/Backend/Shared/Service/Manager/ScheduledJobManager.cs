using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Manifest.Common.Models.DB;
using Manifest.Repository.Common;
using Manifest.Service.App;
using FluentScheduler;
using Framework.Infrastructure.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Manifest.Service.Manager
{
    public class ScheduledJobManager
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILog log;
        private readonly Registry registry;
        private readonly object lockObject = new object();

        public ScheduledJobManager(IServiceScopeFactory scopeFactory, ILog log)
        {
            this.scopeFactory = scopeFactory;
            this.log = log;
            registry = new Registry();
            JobManager.Initialize(registry);
        }

        public async Task InitializeJobsAsync()
        {
            using (var scope = scopeFactory.CreateScope())
            {
                await BuildTasksAsync(scope);
            }
        }

        public void AddJobAsync(ScheduledJob scheduledJob)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                //var repo = scope.ServiceProvider.GetRequiredService<ReportRepository>();

                lock (lockObject)
                {
                    if (AddToRegistry(scheduledJob, GetAction(scheduledJob)))
                    {
                        log.Info($"Added new job: {scheduledJob.JobName}");
                    }
                    else
                    {
                        log.Error($"Error when adding Job - {scheduledJob.JobName} to the registry");
                    }
                }
            }
        }

        private async Task BuildTasksAsync(IServiceScope serviceScope)
        {
            List<ScheduledJob> scheduledJobs;
            var repo = serviceScope.ServiceProvider.GetRequiredService<ReportRepository>();
            scheduledJobs = await repo.GetAllScheduledJobsAsync();

            foreach (var sjob in scheduledJobs)
            {
                if (!AddToRegistry(sjob, GetAction(sjob)))
                    log.Error($"Error when adding Job - {sjob.JobName} to the registry");
            }

            JobManager.Initialize(registry);

            JobManager.JobException += info =>
            {
                log.Fatal(info.Exception, "An error just happened with a scheduled job: ");
            };

            JobManager.Start();

            foreach (var sch in JobManager.AllSchedules)
            {
                log.Info($"{sch.Name} is schedule for {sch.NextRun}");
            }
        }

        private Action GetAction(ScheduledJob scheduledJob)
        {
#pragma warning disable AsyncFixer03 // Fire-and-forget async-void methods or delegates
            Action action = async () =>
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    try
                    {
                        var reportCreationService = scope.ServiceProvider.GetRequiredService<ReportCreationService>();

                        log.Info($"Processing Scheduled Job - {scheduledJob.JobName} - ({scheduledJob.ScheduledJobId})");

                        var job = await reportCreationService.CreateJobFromScheduledJobFromJobServerAsync(scheduledJob.ScheduledJobId);
                        if (!job.IsSuccess)
                        {
                            log.Error(job.ErrorHolder);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex, $"Error when Processing Scheduled Job - {scheduledJob.JobName} - {scheduledJob.ScheduledJobId}");
                    }
                }
            };
#pragma warning restore AsyncFixer03 // Fire-and-forget async-void methods or delegates

            return action;
        }

        private bool AddToRegistry(ScheduledJob scheduledTask, Action action)
        {
            if (scheduledTask.IsWeekly)
            {
                var dayOfWeek = GetDayOfWeek(scheduledTask);
                if (dayOfWeek == null)
                {
                    log.Error($"Day of Week is not valid for the Weekly Job for Scheduled Task - {scheduledTask.ScheduledJobId}");
                    return false;
                }

                Schedule runSchedule;

                if (scheduledTask.RunOnWeekNumber != null)
                {
                    runSchedule = registry.Schedule(action).WithName(scheduledTask.JobName);

                    var everyRun = runSchedule.ToRunEvery(0);

                    switch (scheduledTask.RunOnWeekNumber)
                    {
                        case 1:
                            everyRun.Months().OnTheFirst(dayOfWeek.Value).At(scheduledTask.StartTime.Value.Hour, scheduledTask.StartTime.Value.Minute);
                            break;
                        case 2:
                            everyRun.Months().OnTheSecond(dayOfWeek.Value).At(scheduledTask.StartTime.Value.Hour, scheduledTask.StartTime.Value.Minute);
                            break;
                        case 3:
                            everyRun.Months().OnTheThird(dayOfWeek.Value).At(scheduledTask.StartTime.Value.Hour, scheduledTask.StartTime.Value.Minute);
                            break;
                        case 4:
                            everyRun.Months().OnTheFourth(dayOfWeek.Value).At(scheduledTask.StartTime.Value.Hour, scheduledTask.StartTime.Value.Minute);
                            break;
                        default:
                            log.Error($"RunOn Week Number is not valid for Scheduled Job - {scheduledTask.ScheduledJobId}");
                            break;
                    }
                }
                else
                {
                    runSchedule = registry.Schedule(action).WithName(scheduledTask.JobName).NonReentrant();
                    runSchedule.ToRunEvery(0).Weeks().On(dayOfWeek.Value).At(scheduledTask.StartTime.Value.Hour, scheduledTask.StartTime.Value.Minute);
                }

                log.Info($"Scheduled Weekly Job - {scheduledTask.JobName}");
            }
            else if (scheduledTask.IsDaily)
            {
                var runSchedule = registry.Schedule(action).WithName(scheduledTask.JobName).NonReentrant();
                runSchedule.ToRunEvery(0).Days().At(scheduledTask.RunDailyTime?.Hour ?? 0, scheduledTask.RunDailyTime?.Minute ?? 0);
                log.Info($"Scheduled Daily Job - {scheduledTask.JobName}");
            }
            else if (scheduledTask.IsMonthly)
            {
                var runSchedule = registry.Schedule(action).WithName(scheduledTask.JobName).NonReentrant();
                var schedule = runSchedule.ToRunEvery(0);

                if (scheduledTask.RunMonthlyDay == -1)
                    schedule.Months().OnTheLastDay().At(scheduledTask.RunMonthlyTime?.Hour ?? 1, scheduledTask.RunMonthlyTime?.Minute ?? 30);
                else
                    schedule.Months().On((int)(scheduledTask.RunMonthlyDay ?? 1)).At(scheduledTask.RunMonthlyTime?.Hour ?? 1, scheduledTask.RunMonthlyTime?.Minute ?? 30);

                log.Info($"Scheduled Monthly Job - {scheduledTask.JobName}.");
            }
            else if (scheduledTask.IsHourly)
            {
                var runSchedule = registry.Schedule(action).WithName(scheduledTask.JobName).NonReentrant();
                runSchedule.ToRunEvery(0).Hours().At((int)(scheduledTask.RunHourlyMinute ?? 1));

                log.Info($"Scheduled Hourly Job - {scheduledTask.JobName}");
            }
            else
            {
                log.Error("Invalid week day provided");
                return false;
            }

            return true;
        }

        private DayOfWeek? GetDayOfWeek(ScheduledJob scheduledTask)
        {
            if (scheduledTask.RunSunday)
                return DayOfWeek.Sunday;
            if (scheduledTask.RunMonday)
                return DayOfWeek.Monday;
            if (scheduledTask.RunTuesday)
                return DayOfWeek.Tuesday;
            if (scheduledTask.RunWednesday)
                return DayOfWeek.Wednesday;
            if (scheduledTask.RunThursday)
                return DayOfWeek.Thursday;
            if (scheduledTask.RunFriday)
                return DayOfWeek.Friday;
            if (scheduledTask.RunSaturday)
                return DayOfWeek.Saturday;
            return null;
        }
    }
}
