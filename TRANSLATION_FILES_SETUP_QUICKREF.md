# Quick Reference: Translation Files in Setup/Installation

## What Was Done

The translation JSON files are now properly configured to be included in any installation or setup package for the Stock Manager application.

## Key Changes

### 1. UI.csproj Updated ✅
Translation files changed from `<None>` to `<Content>`:
- `Translations/es.json` (Spanish)
- `Translations/en.json` (English)
- `Translations/README.md` (Documentation)

**This ensures the files are included in ALL deployment scenarios.**

### 2. Documentation Created ✅

- **[DEPLOYMENT_TRANSLATION_FILES.md](DEPLOYMENT_TRANSLATION_FILES.md)** - Complete deployment guide
- **[UI/Properties/PublishProfiles/README.md](UI/Properties/PublishProfiles/README.md)** - Publish profile instructions
- **[SETUP.md](SETUP.md)** - Updated with deployment section

### 3. Publish Profile Template ✅

Template provided at: `UI/Properties/PublishProfiles/FolderProfile.pubxml.template`

## Quick Start: Deploying Your App

### Option A: Visual Studio (Easiest)

1. Open solution in Visual Studio
2. Right-click `UI` project → **Publish...**
3. Choose your deployment method:
   - **Folder** - For creating a distributable package
   - **ClickOnce** - For web-based installation with auto-updates
4. **IMPORTANT:** If using ClickOnce, click "Application Files..." and verify:
   - `Translations\en.json` → Include ✓
   - `Translations\es.json` → Include ✓
5. Click **Publish**

### Option B: Command Line (Advanced)

1. Copy the publish profile template:
   ```cmd
   copy UI\Properties\PublishProfiles\FolderProfile.pubxml.template UI\Properties\PublishProfiles\FolderProfile.pubxml
   ```

2. Build and publish:
   ```cmd
   msbuild tp_diploma_nk_2026.sln /p:Configuration=Release
   msbuild UI\UI.csproj /t:Publish /p:PublishProfile=FolderProfile
   ```

3. Find your files in: `UI\bin\Publish\`

### Option C: Manual Copy (Simple)

1. Build the solution in Release mode
2. Copy everything from `UI\bin\Release\` to your deployment folder
3. The `Translations` folder is automatically included

## Verify It Worked

After deploying, check that these files exist in your installation directory:

```
YourInstallationFolder\
├── UI.exe
├── UI.exe.config
├── Translations\          ← This folder must be here!
│   ├── en.json           ← English translations
│   ├── es.json           ← Spanish translations
│   └── README.md
└── (other DLLs)
```

## Test the Translations

1. Run the installed application
2. Log in with your credentials
3. Go to: **Settings** → **Language**
4. Switch between English and Spanish
5. Verify menu items and labels change language

## Troubleshooting

### Problem: UI shows "Common.Login" instead of "Login"

**Cause:** Translation files are missing

**Solution:**
1. Check if `Translations` folder exists next to `UI.exe`
2. Verify `en.json` and `es.json` are in the folder
3. Republish and ensure files are included

### Problem: Files not in ClickOnce package

**Solution:**
1. In Visual Studio, go to Publish settings
2. Click "Application Files..."
3. Set translation files to "Include"
4. Republish

### Problem: Only English works (or only Spanish)

**Solution:**
1. Check that both `en.json` and `es.json` are present
2. Open the files and verify they're not corrupted
3. Compare with originals in the repository

## Need More Help?

See the complete guides:
- **[DEPLOYMENT_TRANSLATION_FILES.md](DEPLOYMENT_TRANSLATION_FILES.md)** - Detailed deployment instructions
- **[SETUP.md](SETUP.md)** - Installation and setup guide
- **[MULTILANG_IMPLEMENTATION.md](MULTILANG_IMPLEMENTATION.md)** - How translations work

## For Installer Developers

If you're creating a custom installer (MSI, WiX, InstallShield, etc.):

**Critical Requirement:** Include the entire `Translations` folder in your installer package.

The folder must be installed at the same level as the main executable:
```
C:\Program Files\YourCompany\StockManager\
├── UI.exe
└── Translations\
    ├── en.json
    └── es.json
```

See the [WiX example in DEPLOYMENT_TRANSLATION_FILES.md](DEPLOYMENT_TRANSLATION_FILES.md#method-3-creating-an-msi-installer) for implementation details.

## Summary

✅ **Translation files are now properly configured**
✅ **They will be automatically included in standard deployments**
✅ **Comprehensive documentation is available**
✅ **Multiple deployment options are supported**

The system is ready for deployment - just follow one of the methods above!
