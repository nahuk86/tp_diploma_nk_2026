# Deployment Guide - Including Translation Files

## Overview
This guide explains how to deploy the Stock Manager application and ensure that translation JSON files are properly included in your installation package.

## Translation Files Location
The application uses JSON files for multi-language support:
- `UI/Translations/en.json` - English translations
- `UI/Translations/es.json` - Spanish translations
- `UI/Translations/README.md` - Translation documentation

These files MUST be included in any deployment package for the UI labels and messages to display correctly.

## Deployment Methods

### Method 1: Folder Publishing (Recommended for Manual Distribution)

This method creates a folder with all necessary files that can be zipped and distributed.

**Using Visual Studio:**
1. Open the solution in Visual Studio
2. Right-click on the `UI` project
3. Select `Publish...`
4. Choose `Folder` as the target
5. Set the publish location (default: `bin\Publish\`)
6. Click `Publish`

**Using MSBuild (Command Line):**

First, create a publish profile from the template:
```cmd
copy UI\Properties\PublishProfiles\FolderProfile.pubxml.template UI\Properties\PublishProfiles\FolderProfile.pubxml
```

Then publish:
```cmd
msbuild UI\UI.csproj /t:Publish /p:Configuration=Release /p:PublishProfile=FolderProfile
```

**Note:** A publish profile template is provided at `UI/Properties/PublishProfiles/FolderProfile.pubxml.template`. See the README in that directory for more details.

**Verify Translation Files:**
After publishing, verify that the `Translations` folder is present in the publish directory:
```
bin\Publish\
├── UI.exe
├── UI.exe.config
├── Translations\
│   ├── en.json
│   ├── es.json
│   └── README.md
└── (other DLLs and files)
```

### Method 2: ClickOnce Publishing (For Automatic Updates)

ClickOnce is useful for distributing applications that can be automatically updated.

**Setup ClickOnce in Visual Studio:**
1. Right-click on the `UI` project → `Properties`
2. Go to the `Publish` tab
3. Set the Publishing Folder Location (e.g., `\\server\share\StockManager\`)
4. Click `Application Files...` button
5. Ensure the following files are set to `Include`:
   - `Translations\en.json` - Include
   - `Translations\es.json` - Include
   - `Translations\README.md` - Include (Data File)
6. Set `Install Mode` (Online only or Online/Offline)
7. Click `Publish Now`

**Important:** The translation files are now marked as `<Content>` in the project file, which means they will automatically be included in ClickOnce deployments.

### Method 3: Creating an MSI Installer

If you're using a setup project or installer tool (like WiX, InstallShield, Advanced Installer):

**Key Requirements:**
1. Include the entire `Translations` folder in your installer
2. Ensure files are copied to the application installation directory
3. The `Translations` folder should be at the same level as the main executable

**Example Directory Structure:**
```
C:\Program Files\StockManager\
├── UI.exe
├── UI.exe.config
├── Translations\
│   ├── en.json
│   ├── es.json
│   └── README.md
├── BLL.dll
├── DAO.dll
├── DOMAIN.dll
└── SERVICES.dll
```

**WiX Example:**
```xml
<Component Id="TranslationsFolder" Guid="YOUR-GUID-HERE">
  <File Id="EnglishTranslation" Source="$(var.UI.TargetDir)Translations\en.json" KeyPath="yes" />
  <File Id="SpanishTranslation" Source="$(var.UI.TargetDir)Translations\es.json" />
  <File Id="TranslationReadme" Source="$(var.UI.TargetDir)Translations\README.md" />
</Component>
```

### Method 4: Manual (XCopy) Deployment

For simple deployments without an installer:

1. Build the project in Release mode
2. Copy the entire contents of `UI\bin\Release\` to the target machine
3. **CRITICAL:** Ensure the `Translations` folder is copied with all JSON files
4. Place all files in the same directory structure

**PowerShell Script for Manual Deployment:**
```powershell
# Build the project
msbuild tp_diploma_nk_2026.sln /p:Configuration=Release

# Create deployment package
$deployPath = ".\Deploy\StockManager"
New-Item -ItemType Directory -Force -Path $deployPath

# Copy main files
Copy-Item "UI\bin\Release\*" -Destination $deployPath -Recurse -Force

# Verify translations are included
if (Test-Path "$deployPath\Translations\en.json") {
    Write-Host "✓ English translations included"
} else {
    Write-Error "✗ English translations MISSING!"
}

if (Test-Path "$deployPath\Translations\es.json") {
    Write-Host "✓ Spanish translations included"
} else {
    Write-Error "✗ Spanish translations MISSING!"
}
```

## Verification After Deployment

### 1. Check File Presence
After deployment, verify the `Translations` folder exists in the application directory:
- Windows Explorer: Navigate to installation directory and check for `Translations` folder
- Command line: `dir Translations\*.json`

### 2. Test Application Language Switching
1. Launch the application
2. Log in with admin credentials
3. Navigate to `Settings` → `Language`
4. Switch between English and Spanish
5. Verify that:
   - Menu items update correctly
   - Form labels change language
   - Error messages appear in the selected language

### 3. Check Application Logs
If translations are not loading, check the application logs:
- Default location: `Logs\` folder next to the executable
- Look for errors related to "Translations" or "Localization"

## Troubleshooting

### Problem: UI labels appear as keys (e.g., "Common.Login" instead of "Login")

**Cause:** Translation files are missing or not in the correct location.

**Solution:**
1. Verify `Translations` folder exists in the same directory as `UI.exe`
2. Confirm `en.json` and `es.json` are present in the `Translations` folder
3. Check file permissions (files must be readable)
4. Verify JSON files are not corrupted (open in a text editor)

### Problem: Only some translations work

**Cause:** JSON files are incomplete or have syntax errors.

**Solution:**
1. Open the JSON file and validate its structure
2. Use a JSON validator (e.g., jsonlint.com)
3. Compare with the original files in the repository
4. Ensure all required translation keys are present

### Problem: Translations don't load after ClickOnce update

**Cause:** Translation files were not marked as "Include" in ClickOnce deployment.

**Solution:**
1. In Visual Studio, go to Project Properties → Publish tab
2. Click "Application Files..."
3. Find `Translations\en.json` and `Translations\es.json`
4. Set their status to "Include"
5. Republish the application

### Problem: Build succeeds but Translations folder is empty in output

**Cause:** Project file configuration issue.

**Solution:**
1. Open `UI\UI.csproj` in a text editor
2. Verify the translation files are marked as `<Content>`:
```xml
<Content Include="Translations\es.json">
  <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
</Content>
<Content Include="Translations\en.json">
  <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
</Content>
```
3. Rebuild the solution

## CI/CD Integration

If you're using automated build/deployment pipelines:

### Azure DevOps / GitHub Actions
Ensure your build pipeline includes the Translations folder in the artifact:

```yaml
# Example for Azure Pipelines
- task: CopyFiles@2
  inputs:
    SourceFolder: '$(Build.SourcesDirectory)/UI/bin/Release'
    Contents: |
      **/*
      Translations/**
    TargetFolder: '$(Build.ArtifactStagingDirectory)'

# Example for GitHub Actions
- name: Package application
  run: |
    cd UI/bin/Release
    zip -r ../../../StockManager.zip . -i "*" "Translations/*"
```

## Adding New Languages

To add support for additional languages:

1. Create a new JSON file in `UI/Translations/` (e.g., `fr.json` for French)
2. Copy the structure from `en.json` or `es.json`
3. Translate all values
4. Add the file to the project in Visual Studio
5. Set its properties:
   - Build Action: `Content`
   - Copy to Output Directory: `Copy if newer` or `Copy always`
6. Update `UI.csproj` if needed:
```xml
<Content Include="Translations\fr.json">
  <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
</Content>
```
7. Rebuild and republish

## Best Practices

1. **Always verify** translation files after publishing
2. **Test language switching** before distributing to users
3. **Version control** all translation files
4. **Backup** translation files separately before updates
5. **Document** any custom translations for your deployment
6. **Validate** JSON syntax before deployment
7. **Keep** README.md with translations for future reference

## Summary

The translation files are now properly configured as `<Content>` items in the project file, which ensures they are:
- ✅ Copied to the build output directory
- ✅ Included in ClickOnce deployments
- ✅ Available for installer projects to include
- ✅ Part of the application's content for any deployment method

No additional setup is required for standard deployments - the files will automatically be included when you publish or build the application.
