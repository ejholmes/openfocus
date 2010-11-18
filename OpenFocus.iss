;
; Script generated by the ASCOM Driver Installer Script Generator 1.3.0.0
; Generated by Eric Holmes on 10/26/2010 (UTC)
;
[Setup]
AppName=ASCOM OpenFocus Focuser Driver
AppVerName=ASCOM OpenFocus Focuser Driver 0.1
AppVersion=0.1
AppPublisher=Eric Holmes <eric@ejholmes.net>
AppPublisherURL=mailto:eric@ejholmes.net
AppSupportURL=http://tech.groups.yahoo.com/group/ASCOM-Talk/
AppUpdatesURL=http://ascom-standards.org/
VersionInfoVersion=1.0.0
MinVersion=0,5.0.2195sp4
DefaultDirName="{pf}\OpenFocus"
DefaultGroupName=OpenFocus Driver
DisableDirPage=no
DisableProgramGroupPage=yes
OutputDir="."
OutputBaseFilename="OpenFocus Setup"
Compression=lzma/Normal
SolidCompression=false
; Put there by Platform if Driver Installer Support selected
WizardImageFile="C:\Program Files\ASCOM\InstallGen\Resources\WizardImage.bmp"
LicenseFile="C:\Program Files\ASCOM\InstallGen\Resources\CreativeCommons.txt"
; {cf}\ASCOM\Uninstall\Focuser folder created by Platform, always
UninstallFilesDir="{cf}\ASCOM\Uninstall\Focuser\OpenFocus"
PrivilegesRequired=admin
ArchitecturesInstallIn64BitMode=x64 ia64

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Dirs]
Name: "{cf}\ASCOM\Uninstall\Focuser\OpenFocus"
; TODO: Add subfolders below {app} as needed (e.g. Name: "{app}\MyFolder")

[Icons]
Name: "{group}\Uninstall OpenFocus Driver"; Filename: "{uninstallexe}"

;  Add an option to install the source files
[Tasks]
Name: ascom; Description: Install the ASCOM driver;
Name: win32; Description: Install the Windows driver;
Name: source; Description: Install the Source files; Flags: unchecked

[Files]
Source: "ASCOM\OpenFocus\bin\Release\ASCOM.OpenFocus.dll"; DestDir: "{app}"; Tasks: ascom
Source: "ASCOM\OpenFocus\bin\Release\openfocus.dll"; DestDir: "{app}"; Tasks: ascom
; Require a read-me HTML to appear after installation, maybe driver's Help doc
Source: "README.markdown"; DestDir: "{app}"; Flags: isreadme; Tasks: ascom
; Optional source files (COM and .NET aware)
Source: "ASCOM\OpenFocus\*"; Excludes: *.zip,*.exe,*.dll, \bin\*, \obj\*, \driver\*; DestDir: "{app}\Source\ASCOM"; Tasks: source; Flags: recursesubdirs

; copy your libusb-win32 setup package to the App folder
Source: "Win32\driver\*"; Excludes: "*.exe"; Flags: recursesubdirs; DestDir: "{app}\driver"

; also copy the native (32bit or 64 bit) libusb0.dll to the 
; system folder so that rundll32.exe will find it
Source: "Win32\driver\x86\libusb0_x86.dll"; DestName: "libusb0.dll"; DestDir: "{sys}"; Flags: uninsneveruninstall replacesameversion restartreplace promptifolder; Check: IsX86; Tasks: win32
Source: "Win32\driver\amd64\libusb0.dll"; DestDir: "{sys}"; Flags: uninsneveruninstall replacesameversion restartreplace promptifolder; Check: IsX64; Tasks: win32
Source: "Win32\driver\ia64\libusb0.dll"; DestDir: {sys}; Flags: uninsneveruninstall replacesameversion restartreplace promptifolder; Check: IsI64; Tasks: win32

; Only if driver is .NET
[Run]
; touch the HID .inf file to break its digital signature
; this is only required if the device is a mouse or a keyboard !!
;Filename: "rundll32"; Parameters: "libusb0.dll,usb_touch_inf_file_np_rundll {win}\inf\input.inf"

; invoke libusb's DLL to install the .inf file
Filename: "rundll32"; Parameters: "libusb0.dll,usb_install_driver_np_rundll {app}\driver\OpenFocus.inf"; StatusMsg: "Installing driver (this may take a few seconds) ..."; Tasks: win32

; Only for .NET assembly/in-proc drivers
Filename: "{%FrameworkDir|{win}\Microsoft.NET\Framework}\V2.0.50727\regasm.exe"; Parameters: "/codebase ""{app}\ASCOM.OpenFocus.dll"""; Flags: runhidden; StatusMsg: "Registering driver with ASCOM..."; Tasks: ascom

; Only if driver is .NET
[UninstallRun]
; Only for .NET assembly/in-proc drivers
Filename: "{%FrameworkDir|{win}\Microsoft.NET\Framework}\V2.0.50727\regasm.exe"; Parameters: "-u ""{app}\ASCOM.OpenFocus.dll"""; Flags: runhidden



[CODE]
//
// Before the installer UI appears, verify that the (prerequisite)
// ASCOM Platform 5 or greater is installed, including both Helper
// components. Helper is required for all types (COM and .NET)!
//
function InitializeSetup(): Boolean;
var
   H : Variant;
   H2 : Variant;
begin
   Result := FALSE;  // Assume failure
   try               // Will catch all errors including missing reg data
      H := CreateOLEObject('DriverHelper.Util');  // Assure both are available
      H2 := CreateOleObject('DriverHelper2.Util');
      if (H2.PlatformVersion >= 5.0) then
         Result := TRUE;
   except
   end;
   if(not Result) then
      MsgBox('The ASCOM Platform 5 or greater is required for this driver.', mbInformation, MB_OK);
end;

function IsX64: Boolean;
begin
  Result := Is64BitInstallMode and (ProcessorArchitecture = paX64);
end;

function IsI64: Boolean;
begin
  Result := Is64BitInstallMode and (ProcessorArchitecture = paIA64);
end;

function IsX86: Boolean;
begin
  Result := not IsX64 and not IsI64;
end;

function Is64: Boolean;
begin
  Result := IsX64 or IsI64;
end;
