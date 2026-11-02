@echo off
cd /d "%~dp0"
"%WINDIR%\SysWOW64\WindowsPowerShell\v1.0\powershell.exe" -NoProfile -ExecutionPolicy Bypass -File "%~dp0export_access.ps1" > "%~dp0export_log.txt" 2>&1
