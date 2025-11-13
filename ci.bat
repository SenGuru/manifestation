@ECHO OFF
rem throw the first parameter away
rem shift
set firstParam=%1
set emptyParam=

if "%~1"=="" (    
    set firstParam=""
)

set params=--Command=%firstParam%
SET paramValue=1
SET _is_prev_command=0
SET _startchar=0
SET _length=2
SET _arg=%firstParam%
SET paramStart=--
CALL SET _substring=%%_arg:~%_startchar%,%_length%%%

 rem echo sub string=%_substring%
 rem echo before_params=%params%
 rem echo _is_prev_command=%_is_prev_command%

 if %_substring% == %paramStart% (
    set params=%params%=
    set _is_prev_command=1
)

rem echo after_params=%params%

:loop

shift
if [%1]==[] goto afterloop

SET _startchar=0
SET _length=2
SET _arg=%1
SET paramStart=--
CALL SET _substring=%%_arg:~%_startchar%,%_length%%%

rem echo sub string=%_substring%
rem echo before_params=%params%
rem echo _is_prev_command=%_is_prev_command%

if %_substring% == %paramStart% (
    set params=%params% %1=
    set _is_prev_command=1
) else (
    if %_is_prev_command% == %paramValue% (
        set params=%params%%1
    ) else (
        set params=%params% %1
    )
    set _is_prev_command=0
)

rem echo after_params=%params%

goto loop
:afterloop

pushd .\ci

ECHO Running CI Tool
ECHO 
ECHO Restoring Cake tool

dotnet tool restore

ECHO Running Cake with param = %params%

call dotnet cake %params%

popd

if errorlevel 1 (
   echo Failure Reason Given is %errorlevel%
   exit /b %errorlevel%
)

@ECHO ON

EXIT /B %error_leval%