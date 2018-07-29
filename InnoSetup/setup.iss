
#define use_dotnetfx40

#define use_msiproduct
#define use_vc2010


#define MyAppSetupName 'Comic Reader X'
#define MyAppVersion '1.0'

[Setup]
AppName={#MyAppSetupName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppSetupName} {#MyAppVersion}
AppCopyright=Copyright © 2012-2017 stfx
VersionInfoVersion={#MyAppVersion}
VersionInfoCompany=stfx
AppPublisher=Comic Reader X
;AppPublisherURL=http://...
;AppSupportURL=http://...
;AppUpdatesURL=http://...
OutputBaseFilename={#MyAppSetupName}-{#MyAppVersion}
DefaultGroupName={#MyAppSetupName}
DefaultDirName={pf}\{#MyAppSetupName}
UninstallDisplayIcon={app}\MyProgram.exe
OutputDir=bin
SourceDir=.
AllowNoIcons=yes
;SetupIconFile=MyProgramIcon
SolidCompression=yes

;MinVersion default value: "0,5.0 (Windows 2000+) if Unicode Inno Setup, else 4.0,4.0 (Windows 95+)"
MinVersion=0,5.0
PrivilegesRequired=admin
ArchitecturesAllowed=x86 x64
ArchitecturesInstallIn64BitMode=x64

; downloading and installing dependencies will only work if the memo/ready page is enabled (default and current behaviour)
DisableReadyPage=no
DisableReadyMemo=no

; supported languages
#include "scripts\lang\english.iss"
                             
[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "src\dotNetFx40_Full_setup.exe"; Flags: dontcopy
Source: "src\MyProgram-x64.exe"; DestDir: "{app}"; DestName: "MyProgram.exe"; Check: IsX64
Source: "src\MyProgram-IA64.exe"; DestDir: "{app}"; DestName: "MyProgram.exe"; Check: IsIA64
Source: "src\MyProgram.exe"; DestDir: "{app}"; Check: not Is64BitInstallMode

[Icons]
Name: "{group}\{#MyAppSetupName}"; Filename: "{app}\MyProgram.exe"
Name: "{group}\{cm:UninstallProgram,{#MyAppSetupName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppSetupName}"; Filename: "{app}\MyProgram.exe"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppSetupName}"; Filename: "{app}\MyProgram.exe"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\MyProgram.exe"; Description: "{cm:LaunchProgram,{#MyAppSetupName}}"; Flags: nowait postinstall skipifsilent

[CustomMessages]
DependenciesDir=MyProgramDependencies


; shared code for installing the products
#include "scripts\products.iss"

; helper functions
#include "scripts\products\stringversion.iss"
#include "scripts\products\winversion.iss"
#include "scripts\products\dotnetfxversion.iss"

; actual products
#ifdef use_dotnetfx40
#include "scripts\products\dotnetfx40client.iss"
#include "scripts\products\dotnetfx40full.iss"
#endif

#ifdef use_msiproduct
#include "scripts\products\msiproduct.iss"
#endif
#ifdef use_vc2010
#include "scripts\products\vcredist2010.iss"
#endif

[Code]
function InitializeSetup(): boolean;
begin
ExtractTemporaryFile('dotNetFx40_Full_setup.exe');

initwinversion();
            	


dotnetfx40client();
  


#ifdef use_vc2010
	vcredist2010('10');
#endif
             
	Result := true;
end;
