@echo off
cd C:\Windows\Microsoft.NET\Framework\v4.0.30319
installutil.exe -u "D:\Workspace\efms\src\WebAPI\eFMSWindowService\bin\Debug\eFMSWindowService.exe"

if ERRORLEVEL 1 goto error
exit
:error
echo There was a problem
pause