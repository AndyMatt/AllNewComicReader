[Code]
function FrameworkIsNotInstalled(): Boolean;
var
  bSuccess: Boolean;
  regVersion: Cardinal;
begin
  Result := True;

  bSuccess := RegQueryDWordValue(HKLM, 'Software\Microsoft\NET Framework Setup\NDP\v4\Full', 'Release', regVersion);
  if (True = bSuccess) and (regVersion >= 378389) then begin
    Result := False;
  end;
end;

function CheckInstallRedist32(): Boolean;
var
  bSuccess: Boolean;
begin
  Result := True;
  if IsWin64 then
    Result := False
  else
    if(RegKeyExists(HKLM, 'Software\Classes\Installer\Products\1926E8D15D0BCE53481466615F760A7F')) then
         Result := False;
    
end;

function CheckInstallRedist64(): Boolean;
var
  bSuccess: Boolean;
begin
  Result := True;
  if IsWin64 then begin
     if(RegKeyExists(HKLM, 'Software\Classes\Installer\Products\C173E5AD3336A8D3394AF65D2BB0CCE6')) then
         Result := False
     end;
  
  if not IsWin64 then 
    Result := False;  
end;
 

  procedure InstallFramework;
var
  StatusText: string;
  ResultCode: Integer;
begin
  StatusText := WizardForm.StatusLabel.Caption;
  if FrameworkIsNotInstalled then
  begin
    WizardForm.StatusLabel.Caption := 'Installing .NET framework...';
    WizardForm.ProgressGauge.Style := npbstMarquee;
    ExtractTemporaryFile('dotNetFx40_Full_setup.exe');
    try  
      if not Exec(ExpandConstant('{tmp}\dotNetFx40_Full_setup.exe'), '/q /norestart', '', SW_SHOW, ewWaitUntilTerminated, ResultCode) then
      begin
      { you can interact with the user that the installation failed }
      MsgBox('.NET installation failed with code: ' + IntToStr(ResultCode) + '.',
      mbError, MB_OK);
      end;
    finally
      WizardForm.StatusLabel.Caption := StatusText;
      WizardForm.ProgressGauge.Style := npbstNormal;
    end;

  end;
end;

procedure InstallRedist32;
var
  StatusText: string;
  ResultCode: Integer;
  begin
if CheckInstallRedist32 then
   begin
  StatusText := WizardForm.StatusLabel.Caption;
  WizardForm.StatusLabel.Caption := 'Installing Visual Studio 2010 C++ Redistributable...';
  WizardForm.ProgressGauge.Style := npbstMarquee;
  try  
      ExtractTemporaryFile('vcredist_x86.exe');
      if not Exec(ExpandConstant('{tmp}\vcredist_x86.exe'), '/q /norestart', '', SW_SHOW, ewWaitUntilTerminated, ResultCode) then
      begin
      { you can interact with the user that the installation failed }
      MsgBox('.vcredist 2010 installation failed with code: ' + IntToStr(ResultCode) + '.', mbError, MB_OK);
      end;
  finally
      WizardForm.StatusLabel.Caption := StatusText;
      WizardForm.ProgressGauge.Style := npbstNormal;
  end;
    end;
end;

procedure InstallRedist64;
var
  StatusText: string;
  ResultCode: Integer;
begin
if CheckInstallRedist64 then
begin
  StatusText := WizardForm.StatusLabel.Caption;
  WizardForm.StatusLabel.Caption := 'Installing Visual Studio 2010 C++ Redistributable...';
  WizardForm.ProgressGauge.Style := npbstMarquee;
  try  
      ExtractTemporaryFile('vcredist_x64.exe');
      if not Exec(ExpandConstant('{tmp}\vcredist_x64.exe'), '/q /norestart', '', SW_SHOW, ewWaitUntilTerminated, ResultCode) then
      begin
      { you can interact with the user that the installation failed }
      MsgBox('.vcredist 2010 installation failed with code: ' + IntToStr(ResultCode) + '.', mbError, MB_OK);
      end;
  finally
      WizardForm.StatusLabel.Caption := StatusText;
      WizardForm.ProgressGauge.Style := npbstNormal;
  end;
end;
end;