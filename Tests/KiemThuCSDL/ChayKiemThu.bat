@echo off
chcp 65001 >nul
REM [QA-06] Bien dich va chay kiem thu tang CSDL.
REM Can: da Build chuong trinh (Debug) va SQL Server dang chay. Khong can Visual Studio.
REM Tac gia: BE1 - Nguyen Tuan Anh | Ho tro: Claude (AI) | 15/11/2026
set BIN=%~dp0..\..\Cuahang_Nongduoc\Cuahang Nongduoc\bin\Debug
set CSC=%WINDIR%\Microsoft.NET\Framework\v3.5\csc.exe
if not exist "%BIN%\CuahangNongduoc.exe" (
  echo Chua co "%BIN%\CuahangNongduoc.exe" - hay Rebuild Solution truoc.
  pause & exit /b 1
)
"%CSC%" /nologo /platform:x86 /out:"%BIN%\KiemThuCSDL.exe" /r:"%BIN%\CuahangNongduoc.exe" "%~dp0KiemThuCSDL.cs" || (pause & exit /b 1)
copy /y "%BIN%\CuahangNongduoc.exe.config" "%BIN%\KiemThuCSDL.exe.config" >nul
"%BIN%\KiemThuCSDL.exe"
echo.
pause
