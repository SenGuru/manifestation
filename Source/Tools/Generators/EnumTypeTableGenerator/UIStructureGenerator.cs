using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using System.Text;
using Framework.Infrastructure.Utils;
using CodeGenerator; 

public class UIStructureGeneratorProgram
{
    public class TableConfig
    {
        public string Title { get; set; }
        public string Table { get; set; }
        public string Icon { get; set; }
        public bool IsInfo { get; set; }
        public bool IsTitle { get; set; } = false;
        public List<ConfigOption> Tabs { get; set; } = new();
        public List<TableConfig> Child { get; set; } = new();
        public List<ConfigOption> HighLevelButtons { get; set; } = new();
        public List<ConfigOption> ColumnButtons { get; set; } = new();
    }

    public class ConfigOption
    {
        public string Title { get; set; }
        public string Table { get; set; }
        public bool IsInfo { get; set; }
        public string Function { get; set; }
        public bool AskForConfirmation { get; set; }
    }

    public static List<TableConfig> LoadTablesWithTitle(string filePath)
    {
        var json = File.ReadAllText(filePath);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        return JsonSerializer.Deserialize<List<TableConfig>>(json, options);
    }

    static string SplitCamelCaseToString(string s)
    {
        return string.Concat(s.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x : x.ToString())).ToLower();
    }

    static string CapitalizeFirstLetter(string s)
    {
        if (string.IsNullOrEmpty(s)) return s;
        return char.ToUpper(s[0]) + s.Substring(1);
    }

    static string GetCamelCaseTitleString(string s)
    {
        return string.Concat(s.Select((x, i) => i > 0 && char.IsUpper(x) ? "" + x : x.ToString()));
    }

    private static string GetAngularTempDirectory(string dir)
    {
        //return Directory.GetCurrentDirectory() + "\\..\\..\\..\\Web\\src\\common\\models\\";
        return dir + "\\temp";
    }

    private static string GetAngularCustomEnumFilePath(string dir)
    {
        return dir + "\\src\\common\\models\\app-custom-enums.ts";
    }

    private static string GetAngularCustomModelFilePath(string dir)
    {
        return dir + "\\src\\common\\models\\app-custom-model.ts";
    }

    private static string GetAngularEnJsonFilePath(string dir)
    {
        return dir + "\\src\\assets\\i18n\\en.json";
    }

    private static string GetAngularAdminRoutingModelFilePath(string dir)
    {
        return dir + "\\src\\app\\modules\\admin\\admin-routing.module.ts";
    }

    private static string GetAngularAdminMenuFilePath(string dir)
    {
        return dir + "\\src\\app\\layouts\\menus\\admin-menu.ts";
    }

    private static string GetAngularAppConstantFilePath(string dir)
    {
        return dir + "\\src\\common\\constants\\app.constants.ts";
    }

    private static string GetAngularCompanySessionServiceFilePath(string dir)
    {
        return dir + "\\src\\common\\services\\session\\company-session-service.ts";
    }

    private static string GetAngularSharedModuleFilePath(string dir)
    {
        return dir + "\\src\\app\\shared\\shared.module.ts";
    }

    private static string GetAngularListScreenComponentDirectory(string dir)
    {
        return dir + "\\src\\app\\shared\\components\\screen";
    }

    private static string GetAngularCrudScreenComponentDirectory(string dir)
    {
        return dir + "\\src\\app\\shared\\modals";
    } 
     
    public static void MainApp(bool isAngular, string dir)
    {

        try
        {
             
       string filePath = "Config\\UIStructureConfiguration.json";

       var tablesWithTitle = LoadTablesWithTitle(filePath);
        
        StringBuilder sharedModuleStr = new StringBuilder();

        Console.WriteLine("***** Generating shared modules ********* ");
        
        sharedModuleStr.Append(GenerateContentForSharedModuleComponentListWithChild(tablesWithTitle));

        string cleanedSharedModuleStr = RemoveDuplicateWordInCommaSeparatedString(sharedModuleStr.ToString());

        UpdateSharedModuleFile(dir: dir,modifiedContent: cleanedSharedModuleStr);

        Console.WriteLine("***** Generating header detail Html component ********* ");

         GenerateComponentsForHeaderHtml(tablesWithTitle, dir);

            Console.WriteLine("***** Generating header detail Ts component ********* ");

            GenerateComponentsForHeaderTs(tablesWithTitle, dir);

        Console.WriteLine("***** Generating header config html changes ********* ");
         
            GenerateComponentsForConfigScreenHtml(tablesWithTitle, dir);

        Console.WriteLine("***** Generating header config Ts changes ********* ");

            GenerateComponentsForConfigScreenTs(tablesWithTitle, dir);

        Console.WriteLine("***** Generating custom model changes ********* ");

        StringBuilder content = new StringBuilder();

        content.AppendLine(GenerateContentModelForCustomWithChild(tablesWithTitle));

        UpdateCustomModelFile(dir: dir,modifiedContent: content.ToString());


        Console.WriteLine("***** Generating custom enum changes ********* ");

        content = new StringBuilder();

        content.AppendLine(GenerateContentForMenuItemsWithChildInCustomModel(tablesWithTitle));

        UpdateCustomEnumFile(dir: dir, modifiedContent: content.ToString());

        Console.WriteLine("***** Generating Company session file changes ********* ");

        content = new StringBuilder();

        content.AppendLine(GenerateContentForCompanySessionService(tablesWithTitle));

        UpdateCompanySessionServiceFile(dir: dir, modifiedContent: content.ToString());

        Console.WriteLine("***** Generating app constant file changes ********* ");

        content = new StringBuilder();

        content.AppendLine(GenerateContentForAppConstantFile(tablesWithTitle));

        UpdateAppConstantFile(dir: dir, modifiedContent: content.ToString());


        Console.WriteLine("***** Generating admin routing module changes ********* ");

        content = new StringBuilder();

        content.Append(@$"
            import {{RouterModule,Routes}} from '@angular/router';
            import {{NgModule}} from '@angular/core';
            import {{LayoutComponent}} from 'src/app/layouts/layout.component';

            const routes: Routes = [
              {{
                path: '',
                component: LayoutComponent,
                children: [
        ");

        content.AppendLine(GenerateContentForRoutingModuleWithChild(tablesWithTitle));
        content.Append(@$" 

              {{ path: ""profile"", component: ProfileComponent }},
              {{ path: '**', redirectTo: 'company-config' }},
              ]
              }},
            ];

        @NgModule({{
          imports: [RouterModule.forChild(routes)],
          exports: [RouterModule]
        }})
        export class AdminRoutingModule {{ }}");


        CodeGenUtils.CleanAndWriteAndOpenFile(GetAngularAdminRoutingModelFilePath(dir), content);

        Console.WriteLine("***** Generating admin menu changes ********* ");
        content = new StringBuilder();

        content.Append(@$"
         import {{ ADMIN_MENU_INITIAL_URL_REPLACEMENT_NAME }} from ""src/common/constants/app.constants"";
         import {{ AdminDetailsMenus }} from ""src/common/models/app-custom-enums"";
         import {{ MenuItem }} from ""src/common/models/app-custom-model"";


         export const ADMIN_MENU: MenuItem[] = [");
        content.AppendLine(GenerateContentForMenuItemsWithChild(tablesWithTitle));
        content.AppendLine(" ];");
        
        CodeGenUtils.CleanAndWriteAndOpenFile(GetAngularAdminMenuFilePath(dir), content);


        Console.WriteLine("***** Generating menu en.json file changes ********* ");

        content = new StringBuilder();

        content.AppendLine(GenerateContentForMenuItemEnJsonWithChild(tablesWithTitle));

        UpdateEnJsonFile(dir: dir, modifiedContent: content.ToString());


    }
        catch (Exception e) { 
            Console.WriteLine(e.ToString());
        }

    }

    static string GenerateContentModelForCustomWithChild(List<TableConfig> configs)
    {
        StringBuilder sb = new StringBuilder();

        foreach (var tableConfig in configs) {

            if (tableConfig.Tabs.Count > 0)
            {
                string screenType = tableConfig.Table.MakeFirstCharAsUpper();
                string stFirstName = SplitCamelCaseToString(screenType).Split('-')[0].MakeFirstCharAsUpper();
                sb.AppendLine(GenerateContentForCustomModel(tableConfig.Tabs, stFirstName));
            }

            if (tableConfig.Child.Count > 0)
            {
                sb.AppendLine(GenerateContentModelForCustomWithChild(tableConfig.Child));
            }
        }

        return sb.ToString();
    }

    static string RemoveDuplicateWordInCommaSeparatedString(string content)
    {
       return string.Join(",\n ",
     content.Split(',')
          .Select(word => word.Trim())
          .Distinct(StringComparer.OrdinalIgnoreCase));

    }

    static void GenerateComponentsForConfigScreenTs(List<TableConfig> configs, string dir)
    {

        foreach (var element in configs)
        {

            if (element.IsTitle)
            {
                continue;
            }

            if(element.Tabs.Count > 0)
            {

            var screenTypeU = element.Table.MakeFirstCharAsUpper();
            var screenTypel = element.Table.MakeFirstCharAsLower();
            var stFirstName = SplitCamelCaseToString(screenTypeU).Split('-')[0];
            var camelCaseSplitName = element.Table.SplitCamelCase("-").ToLower();

            StringBuilder tabContent = new StringBuilder();

            tabContent.AppendLine($@"
import {{ Component,OnInit}} from '@angular/core';
import {{ LoggedInUserModel,  {screenTypeU}Model }} from 'src/common/models/app-model';

import {{CommonDialogService}} from 'src/app/shared/services/common-dialog.service';
import {{CompanySessionService}} from 'src/common/services/session/company-session-service';
import {{ {screenTypeU}ConfigScreenTabTypes}} from 'src/common/models/app-custom-model';
import {{HttpService}} from 'src/common/services/app-http-client';
import {{NavigationService}} from 'src/common/services/navigation/navigation.service';
import {{ObjectUtils}} from 'src/common/framework/common/utils/object.utils';
import {{PredefinedUserTypes}} from 'src/common/models/app-enums';
import {{Router}} from '@angular/router';
import {{RouterDataService}} from 'src/common/services/router/router-data.service';
import {{RouterUtils}} from 'src/common/framework/common/utils/router.utils';
import {{SessionService}} from 'src/common/framework/web/infrastructure/session/session.service';

@Component({{
  selector: 'app-{camelCaseSplitName}-config-screen',
  templateUrl: './{camelCaseSplitName}-config-screen.component.html',
  standalone: false
}})
export class {screenTypeU}ConfigScreenComponent implements OnInit {{

  public companyId: number | null = null;
  public companyName: string | null = null;
  public layoutTypeId: number | null = null;
  public initialUrl = '';
  public currentUser: LoggedInUserModel | null = null;
  public isSuperAdmin: boolean = false;
  public {screenTypel}Id: number | null = null;
  public isViewMode: boolean = false;
  public isSuperAdmin: boolean = false;
  public {screenTypel}Info: {screenTypeU}Model | undefined;

            ");

            for (int index = 0; index < element.Tabs.Count; index++)
            {
                var tab = element.Tabs[index];
                var firstLetterUpperCaseElement = GetCamelCaseTitleString(tab.Title).MakeFirstCharAsUpper();
                tabContent.AppendLine($@"public show{firstLetterUpperCaseElement}TabScreen: boolean = false;");
            }

            tabContent.AppendLine($@"
 
              public constructor(
    private httpService: HttpService,
    private routerDataService: RouterDataService,
    private sessionService: SessionService,
    private router: Router,
    private dialogService: CommonDialogService,
    private navigationService: NavigationService,
    private companySessionService: CompanySessionService,
  ) {{

  }}



  public ngOnInit(): void {{
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.initialUrl = this.sessionService.getInitialUrl();
    const routerParams = this.routerDataService.getRouterParamData(this.router.routerState.snapshot.root);

    if (this.currentUser && this.currentUser.userTypeId && this.currentUser.userTypeId == PredefinedUserTypes.SuperAdmin) {{
      this.isSuperAdmin = true;
      // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
      this.{screenTypel}Id = RouterUtils.getRouterDataNumber(routerParams?.['{screenTypel}Id']) ?? 0;

     if (this.currentUser && this.currentUser.userTypeId && this.currentUser.userTypeId == PredefinedUserTypes.SuperAdmin) {{
          // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
          this.companyId = RouterUtils.getRouterDataNumber(routerParams?.['companyId']) ?? 0;
          this.isSuperAdmin = true;
        }} else {{
          this.companyId = this.currentUser?.companyId ?? 0;
        }}

      const session{screenTypeU} = this.companySessionService.getCurrent{screenTypeU}();
      if (session{screenTypeU} && session{screenTypeU}?.{screenTypel}Id === this.{screenTypel}Id) {{
        this.{screenTypel}Name = session{screenTypeU}?.name;
      }} else {{
        this.get{screenTypeU}Info(this.{screenTypel}Id);
      }}
    }} else {{
      if (this.sessionService.isPermissionEnabled() == true) {{
        //  
      }}
    }}
  }}

  public get{screenTypeU}Info({screenTypel}Id: number): void {{
    this.httpService.get{screenTypeU}({screenTypel}Id).subscribe({{
      next: result => {{
        if (!result.isSuccess) {{
          this.dialogService.errorWithReturnModel(""Unable to get {screenTypeU}"", result);
          return;
        }}
        if (result.model) {{
          this.{screenTypel}Info = result.model;
          this.companySessionService.setCurrent{screenTypeU}(result.model);
        }}
      }},
      error: (err: unknown) => this.dialogService.error(""Unable to get {screenTypeU}""),
      complete: () => null,
    }});
  }}

 public changeTab(tab: {screenTypeU}ConfigScreenTabTypes): void {{
    this.resetTabs();
            ");

            for (int index = 0; index < element.Tabs.Count; index++)
            {
                var tab = element.Tabs[index];
                var titleWithLowerCase = tab.Title.ToLowerInvariant();
                var routingPath = titleWithLowerCase.Replace(" ", "-");
                var firstLetterUpperCaseElement = GetCamelCaseTitleString(tab.Title).MakeFirstCharAsUpper();
                if (index == 0)
                {
                    tabContent.AppendLine($@"
        if (tab == '{routingPath}') {{
          this.show{firstLetterUpperCaseElement}TabScreen = true;
        }}");
                }
                else
                {
                    tabContent.AppendLine($@" else if (tab == '{routingPath}') {{
            this.show{firstLetterUpperCaseElement}TabScreen = true;
        }}");
                }
            }

            tabContent.AppendLine(@"
    }
 public resetTabs(): void {
    ");
            foreach (var tab in element.Tabs)
            {

                var firstLetterUpperCaseElement = GetCamelCaseTitleString(tab.Title).MakeFirstCharAsUpper();
                tabContent.AppendLine($"this.show{firstLetterUpperCaseElement}TabScreen = false;");
            }
            tabContent.AppendLine(@"
    }
}
");


            string dirName = camelCaseSplitName + "-config-screen";
            CreateOrUpdateComponentFile(fileContent: tabContent.ToString(), folderDir: GetAngularListScreenComponentDirectory(dir) + "\\" + dirName, fileName: dirName + ".component.ts");
            }


            if (element.Child.Count > 0)
            {
                GenerateComponentsForConfigScreenTs(element.Child, dir);
            }
        }
         
    }


    static void GenerateComponentsForConfigScreenHtml(List<TableConfig> configs, string dir)
    {

        foreach (var element in configs)
        {

            if (element.IsTitle)
            {
                continue;
            }

            if (element.Tabs.Count > 0)
            {
                var screenTypeU = element.Table.MakeFirstCharAsUpper();
                var screenTypel = element.Table.MakeFirstCharAsLower();
                var stFirstName = SplitCamelCaseToString(screenTypeU).Split('-')[0];
                var camelCaseSplitName = element.Table.SplitCamelCase("-").ToLower();

                StringBuilder tabContent = new StringBuilder();

                tabContent.AppendLine($@"
                <app-{camelCaseSplitName}-header [{screenTypel}Id]=""{screenTypel}Id"" (showChangeTab)=""changeTab($event)""></app-{camelCaseSplitName}-header> 
            ");

                tabContent.AppendLine($@"<div class=""tab-container"">");

                foreach (var tab in element.Tabs)
                {
                    var splittedTableStr = SplitCamelCaseToString(tab.Table);
                    var firstLetterUpperCaseElement = GetCamelCaseTitleString(tab.Title).MakeFirstCharAsUpper();
                    if (tab.IsInfo)
                    {
                        tabContent.AppendLine($@"
            <div *ngIf=""show{firstLetterUpperCaseElement}TabScreen"">
                    <app-{screenTypel}-crud-modal
                        [{screenTypel}Id]=""{screenTypel}Id""  
                        [companyId]=""companyId"" 
                        [companyName]=""companyName""
                [is{screenTypeU}Config]=""true"" [isShowFullScreen]=""true""
                    ></app-{screenTypel}-crud-modal>
            </div>  ");
                    }
                    else
                    {
                        tabContent.AppendLine($@"
            <div *ngIf=""show{firstLetterUpperCaseElement}TabScreen"">
              <app-{splittedTableStr}-list-screen
               [isSuperAdmin]=""isSuperAdmin""
                [is{screenTypeU}Config]=""true"" 
                [isViewMode]=""isViewMode"" [companyId]=""companyId"" [companyName]=""companyName""></app-{splittedTableStr}-list-screen>
            </div>  ");
                    }
                }
                tabContent.AppendLine(@"
        </div>");

                string dirName = camelCaseSplitName + "-config-screen";
                CreateOrUpdateComponentFile(fileContent: tabContent.ToString(), folderDir: GetAngularListScreenComponentDirectory(dir) + "\\" + dirName, fileName: dirName + ".component.html");

            }

            if (element.Child.Count > 0)
            {
                GenerateComponentsForConfigScreenHtml(element.Child, dir);
            }
        }

    }


    static void GenerateComponentsForHeaderTs(List<TableConfig> configs, string dir)
    {
        foreach (var element in configs)
        {

            if (element.IsTitle)
            {
                continue;
            }

            if (element.Tabs.Count > 0)
            {

                var screenTypeU = element.Table.MakeFirstCharAsUpper();
                var screenTypel = element.Table.MakeFirstCharAsLower();
                string stFirstName = screenTypeU.SplitCamelCase().Split(" ")[0];
                string stFirstNameL = stFirstName.MakeFirstCharAsLower();
                string stFirstNameU = stFirstName.MakeFirstCharAsUpper();
                var camelCaseSplitName = element.Table.SplitCamelCase("-").ToLower();

                StringBuilder tabContent = new StringBuilder();

                tabContent.AppendLine($@"
    import {{ Component, EventEmitter, Input, OnInit, Output }} from '@angular/core';
    import {{ {screenTypeU}Model, LoggedInUserModel }} from 'src/common/models/app-model';

    import {{ CommonDialogService }} from 'src/app/shared/services/common-dialog.service';
    import {{ CompanySessionService }} from 'src/common/services/session/company-session-service';
    import {{ {screenTypeU}ConfigScreenTabTypes }} from 'src/common/models/app-custom-model';
    import {{ HttpService }} from 'src/common/services/app-http-client';
    import {{ Router }} from '@angular/router';
    import {{ RouterDataService }} from 'src/common/services/router/router-data.service';
    import {{ RouterUtils }} from 'src/common/framework/common/utils/router.utils';
    import {{ SessionService }} from 'src/common/framework/web/infrastructure/session/session.service';
    import {{ SidebarLayoutTypes }} from 'src/common/models/app-custom-enums';
    import {{ ObjectUtils }} from 'src/common/framework/common/utils/object.utils';

    @Component({{
      selector: 'app-{camelCaseSplitName}-header',
      templateUrl: './{camelCaseSplitName}-header.component.html',
      standalone: false
    }})
    export class {screenTypeU}HeaderComponent implements OnInit {{

      @Output() public showChangeTab: EventEmitter<{screenTypeU}ConfigScreenTabTypes> = new EventEmitter<{screenTypeU}ConfigScreenTabTypes>();
      @Input() public {screenTypel}Id: number = 0;
      @Input() public companyId: number | null = null;
      @Input() public layoutTypeId: number | null = null;
      @Input() public is{stFirstNameU}Config: boolean = false;
      @Input() public {screenTypel}Info: {screenTypeU}Model | undefined;
      public currentUser: LoggedInUserModel | null = null;
      public initialUrl: string | null = null;
      public {screenTypel}Name: string | null = null;
      public isSuperAdmin: boolean = false;
      public selected{screenTypeU}ConfigScreenType: {screenTypeU}ConfigScreenTabTypes | undefined;
              
    ");


                tabContent.AppendLine($@"

  public constructor(private sessionService: SessionService,
    private httpService: HttpService,
    private companySessionService: CompanySessionService,
    private dialogService: CommonDialogService,
    private routerDataService: RouterDataService,
    private router: Router) {{
  }}


  public ngOnInit(): void {{
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.initialUrl = this.sessionService.getInitialUrl();
    const routerParams = this.routerDataService.getRouterParamData(this.router.routerState.snapshot.root);
    const routerData = this.routerDataService.getRouterData(this.router.routerState.snapshot.root);
    // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
    this.is{stFirstNameU}Config = routerData['is{stFirstNameU}Config'];
    // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
    this.{screenTypel}Id = RouterUtils.getRouterDataNumber(routerParams?.['{screenTypel}Id']) ?? 0;

     if (this.currentUser && this.currentUser.userTypeId && this.currentUser.userTypeId == PredefinedUserTypes.SuperAdmin) {{
          // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
          this.companyId = RouterUtils.getRouterDataNumber(routerParams?.['companyId']) ?? 0;
          this.isSuperAdmin = true;
        }} else {{
          this.companyId = this.currentUser?.companyId ?? 0;
        }}

    const session{screenTypeU} = this.companySessionService.getCurrent{screenTypeU}();
    if (session{screenTypeU} && session{screenTypeU}?.{screenTypel}Id === this.{screenTypel}Id) {{
      this.{screenTypel}Name = session{screenTypeU}?.{screenTypel}Name;
      this.{screenTypel}Info = session{screenTypeU};
    }} else {{
      if(ObjectUtils.isObjNullOrUndefined(this.{screenTypel}Info)) {{
        this.get{screenTypeU}Info(this.{screenTypel}Id);
       }}
    }}

    if (this.is{stFirstNameU}Config) {{
      this.changeTab('information');
    }}
  }}

  public get{screenTypeU}PictureUrl({screenTypel}Id: number, profileImageGuid: string, token: string): string {{
    return this.httpService.get{screenTypeU}PictureUrl({screenTypel}Id, profileImageGuid, token);
  }}

  public get{screenTypeU}Info({screenTypel}Id: number): void {{
    this.httpService.get{screenTypeU}({screenTypel}Id).subscribe({{
      next: result => {{
        if (!result.isSuccess) {{
          this.dialogService.errorWithReturnModel(""Unable to get {screenTypeU}"", result);
          return;
        }}
        if (result.model) {{
          this.{screenTypel}Info = result.model;
          this.companySessionService.setCurrent{screenTypeU}(result.model);
        }}
      }},
      error: (err: unknown) => this.dialogService.error(""Unable to get {screenTypeU}""),
      complete: () => null,
    }});
  }}

  ");


                tabContent.AppendLine($@"

   public changeTab(tab: {screenTypeU}ConfigScreenTabTypes): void {{
      this.showChangeTab.emit(tab);
      this.selected{screenTypeU}ConfigScreenType = tab;
    }}


  public backTo{stFirstName}sListScreen(): void {{
    this.router.navigateByUrl(`{GenerateContentForBackNavigationWithChild([element], "${this.initialUrl}")}`);
  }}

"
             );
                tabContent.AppendLine("}");


                string dirName = camelCaseSplitName + "-header";
                CreateOrUpdateComponentFile(fileContent: tabContent.ToString(), folderDir: GetAngularListScreenComponentDirectory(dir) + "\\" + dirName, fileName: dirName + ".component.ts");

            }

            if (element.Child.Count > 0)
            {
                GenerateComponentsForHeaderTs(element.Child, dir);
            }
        }

    }

    static void GenerateComponentsForHeaderHtml(List<TableConfig> configs, string dir)
    {

        foreach (var element in configs)
        {
            string screenTypeU = element.Table.MakeFirstCharAsUpper();
            string screenTypeL = element.Table.MakeFirstCharAsLower();
            string stFirstName = screenTypeU.SplitCamelCase().Split(" ")[0];
            string camelCaseSplitName = element.Table.SplitCamelCase("-").ToLower();

            StringBuilder tabContent = new StringBuilder();

            if (element.IsTitle)
            {
                continue;
            }

            if (element.Tabs.Count > 0)
            {

                tabContent.AppendLine($@"
     <div class=""row"">
       <div class=""col-lg-12"">
         <div class=""card mt-n4 mx-n4"">
            <div class=""bg-warning-subtle"">
                <div class=""card-body pb-0 px-4"">
                    <div class=""row mb-3"">
                        <div class=""col-md"">
                            <div class=""row align-items-center g-3"">
                                <div class=""col-md-auto"">
                                    <div class=""col-auto"">
                                        <div class=""avatar-md"">
                                            <img alt="""" class=""img-thumbnail rounded-circle {camelCaseSplitName}-header-img""  [src]=""get{screenTypeU}PictureUrl({screenTypeL}Id, {screenTypeL}Info?.profileImageGuid ?? '', currentUser?.token)"" />
                                        </div>
                                    </div>
                                </div>
                                <div class=""col-md"">
                                    <div>
                                        <h4 class=""fw-bold""> {{{{{screenTypeL}Name}}}} </h4>
                                        <div class=""hstack gap-3 flex-wrap"">
                                            <div> <i class=""ri-dashboard-fill""></i> Email : {{{{{screenTypeL}Info?.email}}}}
                                            </div>
                                            <div class=""vr""></div> 
                                            <div><i class=""ri-team-line""></i> Company: {{{{{screenTypeL}Info?.companyName}}}}</div>
                                            <div class=""vr""></div> 
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class=""col-md-auto"">
                            <div class=""hstack gap-1 flex-wrap"">
                                <button type=""button"" class=""btn py-0 fs-16 text-body material-shadow-none""
                                    (click)=""backTo{stFirstName}sListScreen();"">
                                    <i class=""ri-arrow-go-back-fill""></i>
                                </button>
                            </div>
                        </div>
                    </div>
                    
                    <div class=""col-xxl-12"">
                    <div class=""card mb-0 shadow-none bg-transparent"">
                        <div class=""card-header align-items-center d-flex bg-transparent"">
                            <div class=""flex-shrink-0 ms-2"">
                                <ul ngbNav
                                    class=""nav justify-content-end nav-tabs-custom rounded card-header-tabs border-bottom-0"">");




                foreach (var tab in element.Tabs)
                {
                    var tableWithUpperCase = tab.Table.ToUpperInvariant();
                    var titleWithLowerCase = tab.Title.ToLowerInvariant();
                    var routingPath = titleWithLowerCase.Replace(" ", "-");
                    tabContent.AppendLine($@"
                    <li class=""nav-item cursor-pointer"" (click)=""changeTab('{routingPath}')"">
                        <a class=""nav-link bg-transparent pt-0 pb-2""
                            [ngClass]=""selected{stFirstName}ConfigScreenType === '{routingPath}' ? 'active' : ''"">
                            {tab.Title.SplitCamelCase()}
                        </a>
                    </li> 
                     ");
                }


                tabContent.AppendLine(@"
                             </ul>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                 <!-- end card body -->
                </div>
            </div>
            <!-- end card -->
        </div>
        <!-- end col -->
     </div>");

                string dirName = camelCaseSplitName + "-header";
                CreateOrUpdateComponentFile(fileContent: tabContent.ToString(), folderDir: GetAngularListScreenComponentDirectory(dir) + "\\" + dirName, fileName: dirName + ".component.html");

            }

            if (element.Child.Count > 0)
            {
                GenerateComponentsForHeaderHtml(element.Child, dir);
            }
        }

    }

    static string GenerateContentForSharedModuleComponentListWithChild(List<TableConfig> configs)
    {
        StringBuilder sb = new StringBuilder();

        foreach (var tableConfig in configs)
        {

            if (!tableConfig.IsTitle)
            {

                sb.AppendLine($"{tableConfig.Table}ListScreenComponent,");
                sb.AppendLine($"{tableConfig.Table}CrudModalComponent,");
                if (tableConfig.Child.Count > 0)
                {
                    sb.AppendLine(GenerateContentForSharedModuleComponentListWithChild(tableConfig.Child));
                }
            }
        }
        return sb.ToString();
    }

    static void CreateOrUpdateComponentFile(string fileContent, string folderDir, string fileName) {

        string filePath = Path.Combine(folderDir, fileName);

        try
        {
            // Create the folder if it doesn't exist
            Directory.CreateDirectory(folderDir);

            // Create the file with some content
            File.WriteAllText(filePath, fileContent);

            Console.WriteLine(@$" {filePath} created successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }

    }

    static void UpdateCompanySessionServiceFile(string modifiedContent, string dir)
    {

        string filePath = GetAngularCompanySessionServiceFilePath(dir);

        UpdateContentBeforeTargetedLine(filePath, targetedLine: "public set", newContent: modifiedContent, isLastLine: false);

    }
    static void UpdateAppConstantFile(string modifiedContent, string dir)
    {

        string filePath = GetAngularAppConstantFilePath(dir);

        UpdateContentBeforeTargetedLine(filePath, targetedLine: "", newContent: modifiedContent, isLastLine: true);

    }

    static void UpdateEnJsonFile(string modifiedContent, string dir)
    {

        string filePath = GetAngularEnJsonFilePath(dir);

        string targetWordForDeclaration = "\"HEADER\": {";

        UpdateContentBeforeTargetedLine(filePath, targetedLine: targetWordForDeclaration, newContent: modifiedContent);

    }

    static void UpdateCustomEnumFile(string modifiedContent, string dir)
    {

        string filePath = GetAngularCustomEnumFilePath(dir);
         
        UpdateContentBeforeTargetedLine(filePath, targetedLine: "", newContent: modifiedContent, isLastLine: true);

    }

    static void UpdateCustomModelFile(string modifiedContent, string dir)
    {

        string filePath = GetAngularCustomModelFilePath(dir);
        string targetWordForDeclaration = "export type";

        UpdateContentBeforeTargetedLine(filePath, targetedLine: targetWordForDeclaration, newContent: modifiedContent);

    }
        static void UpdateSharedModuleFile(string modifiedContent, string dir)
    {

        string importContent = GenerateContentForSharedModuleFileImport(modifiedContent);

        string filePath = GetAngularSharedModuleFilePath(dir);
            string targetWordForDeclaration = "],\r\n  imports: [";
            string targetWordForExport = "],\r\n  schemas:";
            string targetWordForFileImport = "@NgModule({";

        // declaration container
        UpdateContentBeforeTargetedLine(filePath, targetedLine: targetWordForDeclaration, newContent: modifiedContent);
        // export container 
        UpdateContentBeforeTargetedLine(filePath, targetedLine: targetWordForExport, newContent: modifiedContent);
        // import statements
        UpdateContentBeforeTargetedLine(filePath, targetedLine: targetWordForFileImport, newContent: importContent);
       
    }

    static void UpdateContentBeforeTargetedLine(string filePath, string targetedLine, string newContent, bool isLastLine = false)
    {
        // Read the entire file content
        string fileContentDeclaration = File.ReadAllText(filePath);

        // Find the index of the target word
        int Idx;

        if (isLastLine)
        {
            Idx = fileContentDeclaration.Length - 1;
        }
        else
        {
            Idx = fileContentDeclaration.IndexOf(targetedLine);
        }

        if (Idx != -1)
        {
            // Insert the content after the target word
            // int insertPosition = dindex + targetedLine.Length;
            string updatedContent = fileContentDeclaration.Insert(Idx, newContent);

            // Write back to the file (overwrite)
            File.WriteAllText(filePath, updatedContent);

            Console.WriteLine(@$"{filePath} updated");
        }
        else
        {
            Console.WriteLine(@$"Target word not found in this file - {filePath}");
        }
    }


    static string GenerateContentForSharedModuleFileImport(string componentList)
    {
        StringBuilder sb = new StringBuilder();
        string cleanedComponents = RemoveDuplicateWordInCommaSeparatedString(componentList);

        foreach (var component in cleanedComponents.Split(",")) {

            if (component.Length > 0) {

                string componentPath = string.Join('-', component.Replace("Component", "").SplitCamelCase().Split(" ").Select(s => s.Trim().ToLower()));
                var componentType = component.Contains("Modal") ? "modals" : "components/screen";

                sb.AppendLine(@$"import {{ {component} }} from 'src/app/shared/{componentType}/{componentPath}/{componentPath}.component';");
                 
            }
        }
        return sb.ToString();
    }

static string GenerateContentForMenuItemEnJsonWithChild(List<TableConfig> configs)
    {
        StringBuilder sb = new StringBuilder();
        for (int index = 0; index < configs.Count; index++)
        {

            bool isLastIdx = (index == configs.Count - 1);
            var tableConfig = configs[index];

            string tableWithUpperCase = GetCamelCaseTitleString(tableConfig.Title).ToUpperInvariant();
            string configComma = isLastIdx ? "" : ",";

            if (tableConfig.Child.Count > 0)
            {
                 
                sb.AppendLine($@" 
                    ""{tableWithUpperCase}"": {{
                        ""TEXT"": ""{tableConfig.Title}"",
                          ""LIST"": {{
                    ");
                 

                for (int cidx = 0; cidx < tableConfig.Child.Count; cidx++)
                {
                    bool isLastChild = (cidx == tableConfig.Child.Count - 1);
                    var childItem = tableConfig.Child[cidx];
                    string childTableWithUpperCase = GetCamelCaseTitleString(childItem.Title).ToUpperInvariant();
                    string childItemComma = isLastChild ? "" : ",";

                    sb.AppendLine($@" 
                     ""{childTableWithUpperCase}"":  ""{childItem.Title}""{childItemComma}");
                }

                sb.AppendLine("}");

                sb.AppendLine(@$"}},");

            }
            else
            {
                sb.AppendLine($@"
                    ""{tableWithUpperCase}"": {{
                        ""TEXT"": ""{tableConfig.Title}""
                      }},");

            }


        }
        return sb.ToString();
    }


    static string GenerateContentForMenuItemsWithChild(List<TableConfig> configs)
    {
        StringBuilder sb = new StringBuilder();
        for (int index = 0; index < configs.Count; index++)
        {
            var tableConfig = configs[index];

            string screenType;
            
            if(tableConfig.IsInfo)
            {
               screenType = tableConfig.Table.Trim().MakeFirstCharAsUpper() + GetCamelCaseTitleString(tableConfig.Title).Trim().MakeFirstCharAsUpper();
            
            } else
            {
               screenType = GetCamelCaseTitleString(tableConfig.Title).MakeFirstCharAsUpper();
            }


            string tableWithUpperCase = GetCamelCaseTitleString(tableConfig.Title).ToUpperInvariant();

            var menuItemId = $@"AdminDetailsMenus.{screenType}";


            var titleWithLowerCase = tableConfig.Title.ToLowerInvariant();
            var routingPath = titleWithLowerCase.Replace(" ", "-");
           
            if (tableConfig.Child.Count > 0)
            {

               // var crPath = $@"{{ path: '{prPath}/{srPath}/layoutTypeId/{srPath}Id'";
               // var actualPath = $@"{{ADMIN_MENU_INITIAL_URL_REPLACEMENT_NAME}}/";

                sb.AppendLine($@"                 
                  {{
                 id: {menuItemId},
                 label: 'MENUITEMS.{tableWithUpperCase}.TEXT',
                 icon: '{tableConfig.Icon}',
                 isCollapsed: true,
                 subItems: [
                    ");
                


                for (int cidx = 0; cidx < tableConfig.Child.Count; cidx++)
                {
                    var childItem = tableConfig.Child[cidx];
                    string childLableUpperCase = GetCamelCaseTitleString(childItem.Title).ToUpper();
                    string childScreenType;

                    if (childItem.IsInfo)
                    {
                        childScreenType = childItem.Table.Trim().MakeFirstCharAsUpper() + GetCamelCaseTitleString(childItem.Title).MakeFirstCharAsUpper();
                    }
                    else
                    {
                        childScreenType = GetCamelCaseTitleString(tableConfig.Title).MakeFirstCharAsUpper() + GetCamelCaseTitleString(childItem.Title).MakeFirstCharAsUpper();
                    }


                    var childTitleWithLowerCase = childItem.Title.ToLowerInvariant();
                    var ChildRoutingPath = childTitleWithLowerCase.Replace(" ", "-");
                    var childMenuItemId = $@"AdminDetailsMenus.{childScreenType}";

                    sb.AppendLine($@"                 {{
                     id: {childMenuItemId},
                     label: 'MENUITEMS.{childLableUpperCase}.TEXT',
                     parentId: {menuItemId},
                     link: `/${{ADMIN_MENU_INITIAL_URL_REPLACEMENT_NAME}}/{ChildRoutingPath}`,
                  
                     }},");
                }

                sb.AppendLine("]},");

            } else
            {

                if(tableConfig.IsTitle)
                {
                    sb.AppendLine($@"               
                    {{
                     id: {menuItemId},
                     label: 'MENUITEMS.{tableWithUpperCase}.TEXT',
                     isTitle: true,
                     }},");

                } else
                {
                    sb.AppendLine($@"                 
                    {{
                     id: {menuItemId},
                     label: 'MENUITEMS.{tableWithUpperCase}.TEXT',
                     icon: '{tableConfig.Icon}',
                     link: `/${{ADMIN_MENU_INITIAL_URL_REPLACEMENT_NAME}}/{routingPath}`,
                     }},");
                }
            }


        }
        return sb.ToString();
    }


    static string GenerateContentForMenuItemsWithChildInCustomModel(List<TableConfig> configs)
    {
        StringBuilder sb = new StringBuilder();

        var menuIdValue = 0;
        sb.AppendLine("export enum AdminDetailsMenus {");
        for (int index = 0; index < configs.Count; index++)
        {
            var tableConfig = configs[index];

                string screenType;

                if (tableConfig.IsInfo)
                {
                    screenType = tableConfig.Table.Trim().MakeFirstCharAsUpper() + GetCamelCaseTitleString(tableConfig.Title).MakeFirstCharAsUpper();
                }
                else
                {
                    screenType = GetCamelCaseTitleString(tableConfig.Title).MakeFirstCharAsUpper();
                }

                menuIdValue += 1;

                sb.AppendLine($@"{screenType} =  {menuIdValue},");

                for (int cidx = 0; cidx < tableConfig.Child.Count; cidx++)
                {
                    var childItem = tableConfig.Child[cidx];

                    string childScreenType;

                    if (childItem.IsInfo)
                    {
                        childScreenType = childItem.Table.Trim().MakeFirstCharAsUpper() + GetCamelCaseTitleString(childItem.Title).MakeFirstCharAsUpper();
                    }
                    else
                    {
                        childScreenType = GetCamelCaseTitleString(tableConfig.Title).MakeFirstCharAsUpper() + GetCamelCaseTitleString(childItem.Title).MakeFirstCharAsUpper();
                    }

                    menuIdValue += 1;

                    sb.AppendLine($@"{childScreenType} = {menuIdValue},");
            }
        }

        sb.AppendLine("}");
        return sb.ToString();
    }

    static string GenerateContentForRoutingModuleWithChild(List<TableConfig> configs)
    {
        StringBuilder sb = new StringBuilder();

        foreach (var tableConfig in configs)
        {

            string screenType = tableConfig.Table.MakeFirstCharAsUpper();
            string stFirstName = SplitCamelCaseToString(screenType).Split('-')[0];

            string tableWithUpperCase = tableConfig.Table.ToUpperInvariant();
            string stLowerCase = SplitCamelCaseToString(tableConfig.Title);
            string ptLowerCase = SplitCamelCaseToString(tableConfig.Title);
            string prPath = ptLowerCase.Replace(" ", "-");
            string srPath = stLowerCase.Replace(" ", "-");
           
            bool isContainTabs = tableConfig.Tabs.Count > 0;

            string devData = isContainTabs ? $@"data: {{ is{stFirstName.MakeFirstCharAsUpper()}Config: true }}" : "";

            if(!tableConfig.IsTitle)
            {

                if (tableConfig.Tabs.Count > 0)
                {
                    sb.AppendLine($"{{ path: '{prPath}', component: {tableConfig.Table}ConfigScreenComponent , {devData} }},");
                }
                else
                {
                    sb.AppendLine($"{{ path: '{prPath}', component: {tableConfig.Table}ListScreenComponent }},");
                }

                if (tableConfig.Child.Count > 0)
                {

                    var crPath = $@"{{ path: '{prPath}/{srPath}/:layoutTypeId/:{srPath}Id'";

                    sb.AppendLine($@"{crPath}, 

                        children: [

                            {{
                              path: 'info',
                              component: {tableConfig.Table}CrudModalComponent, {devData}
                            }},");

                    foreach (var child in tableConfig.Child)
                    {
                        sb.AppendLine(GenerateContentForRoutingModuleWithChild(child.Child));
                    }

                    sb.AppendLine("],");


                    sb.AppendLine("},");
                }
            }
             
        }
        return sb.ToString();
    }

    static string GenerateContentForBackNavigationWithChild(List<TableConfig> configs, string exisingPath)
    {
        StringBuilder sb = new StringBuilder();

        foreach (var tableConfig in configs)
        {
            string screenType = tableConfig.Table.MakeFirstCharAsUpper();
            string stFirstName = SplitCamelCaseToString(screenType).Split('-')[0];

            string tableWithUpperCase = tableConfig.Table.ToUpperInvariant();
            string stLowerCase = SplitCamelCaseToString(tableConfig.Title);
            string ptLowerCase = SplitCamelCaseToString(tableConfig.Title);
            string prPath = ptLowerCase.Replace(" ", "-");
            string srPath = stLowerCase.Replace(" ", "-");

            sb.Append(exisingPath);
           
            sb.Append($"/{prPath}");

            if (tableConfig.Child.Count > 0)
            {

                foreach (var child in tableConfig.Child)
                {
                    sb.AppendLine(GenerateContentForBackNavigationWithChild(child.Child, sb.ToString()));
                }

            }

        }
        return sb.ToString();
    }

    static string GenerateContentForCompanySessionService(List<TableConfig> configs)
    {
        StringBuilder sb = new StringBuilder();

        if (configs.Count > 0) {

            foreach (var config in configs) { 
            
                if(config.Tabs.Count > 0)
                {

                    var screenTypeU = config.Table.MakeFirstCharAsUpper();
                    var screenTypel = config.Table.MakeFirstCharAsLower(); 

                    sb.AppendLine($@"
        
                        public setCurrent{screenTypeU}({screenTypel}: {screenTypeU}Model): void {{
                            try {{
                                if (!ObjectUtils.isObjNullOrUndefined({screenTypel})) {{
                                    sessionStorage.setItem(SESSION_KEYS.{screenTypel.ToUpper()}_INFORMATION_SESSION_STORAGE_NAME, JSON.stringify({screenTypel}));
                                }}
                            }} catch (e) {{
                                this.dialogService.error(""Fatal Error: Unable to set current {screenTypeU} Information"");
                            }}
                        }}

                        public getCurrent{screenTypeU}(): {screenTypeU}Model | null {{
                            try {{
                                const sessionData = sessionStorage.getItem(SESSION_KEYS.{screenTypeU.ToUpper()}_INFORMATION_SESSION_STORAGE_NAME);
                                if (sessionData) {{
                                    return JSON.parse(sessionData) as {screenTypeU}Model;
                                }} else {{
                                    return null;
                                }}
                            }} catch (e) {{
                                this.dialogService.error(""Fatal Error: Unable to get current {screenTypeU} Information"");
                                return null;
                            }}
                        }}
    
                        public clearCurrent{screenTypeU}Session(): void {{
                            sessionStorage.removeItem(SESSION_KEYS.{screenTypeU.ToUpper()}_INFORMATION_SESSION_STORAGE_NAME);
                        }}

                    ");

                }

                if (config.Child.Count > 0)
                {
                    sb.AppendLine(GenerateContentForCompanySessionService(config.Child));
                }

            }

        }

        return sb.ToString().Trim();
    }

    static string GenerateContentForAppConstantFile(List<TableConfig> configs)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($@"
                            export const SESSION_KEYS = {{ ");
      
        if (configs.Count > 0)
        {

            foreach (var config in configs)
            {

                if (config.Tabs.Count > 0)
                {

                    var screenTypeU = config.Table.MakeFirstCharAsUpper();
                    var screenTypel = config.Table.MakeFirstCharAsLower();

                    sb.AppendLine($@"
                                {screenTypeU.ToUpper()}_INFORMATION_SESSION_STORAGE_NAME: '{screenTypel}InformationSession'
                    ");

                }

                if (config.Child.Count > 0)
                {
                    sb.AppendLine(GenerateContentForAppConstantFile(config.Child));
                }

            }

        }
        
        sb.AppendLine($@"
                            }}");

        return sb.ToString().Trim();
    }

    static string GenerateContentForCustomModel(List<ConfigOption> tabs, string screenType)
    {
        string tabsTypesStr = "";
        if (tabs.Count > 0)
        {
            tabsTypesStr = $"export type {screenType}ConfigScreenTabTypes = ";
            for (int index = 0; index < tabs.Count; index++)
            {
                var element = tabs[index];

                var titleWithLowerCase = element.Title.ToLowerInvariant();
                var routingPath = titleWithLowerCase.Replace(" ", "-");

                bool lastIdx = (index == tabs.Count - 1);

                var lastTxt = lastIdx ? ";" : "|";
                tabsTypesStr += $" '{routingPath}' {lastTxt}";
                 
            }
        }
        return tabsTypesStr;
    }
}