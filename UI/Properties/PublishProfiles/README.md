# Publish Profiles Directory

This directory contains templates and configurations for publishing the Stock Manager application.

## About Publish Profiles

Publish profiles (`.pubxml` files) are **excluded from version control** by `.gitignore` because they may contain:
- Server paths
- Credentials
- Environment-specific settings

However, template files are provided for your convenience.

## Available Templates

### FolderProfile.pubxml.template

A template for publishing to a local folder for distribution.

**To use:**
1. Copy `FolderProfile.pubxml.template` to `FolderProfile.pubxml`
2. Edit the `<PublishDir>` element if you want a different output location
3. Publish using:
   - **Visual Studio:** Right-click UI project → Publish → Select profile
   - **Command line:** `msbuild UI\UI.csproj /t:Publish /p:Configuration=Release /p:PublishProfile=FolderProfile`

**Important:** The translation JSON files (`en.json`, `es.json`) are automatically included in the publish output because they are configured as `<Content>` items in `UI.csproj`.

## Creating Your Own Publish Profiles

### For Folder Publishing
Use the template provided above as a starting point.

### For ClickOnce Publishing
1. In Visual Studio, right-click the UI project
2. Select **Publish...**
3. Choose **ClickOnce** as the target
4. Configure your settings (URL, installation folder, etc.)
5. Click **Application Files...** and verify that:
   - `Translations\en.json` is set to **Include**
   - `Translations\es.json` is set to **Include**
6. Click **Publish**

Visual Studio will create a `.pubxml` file automatically in this directory.

### For Web Deploy / FTP
Follow similar steps in Visual Studio's Publish wizard, choosing the appropriate target.

## Verification

After publishing, always verify that the `Translations` folder is present in your publish output:

```
Published Output\
├── UI.exe
├── UI.exe.config
├── Translations\        ← Must be present
│   ├── en.json
│   ├── es.json
│   └── README.md
└── (other DLLs)
```

If the Translations folder is missing, see the troubleshooting section in `DEPLOYMENT_TRANSLATION_FILES.md`.

## More Information

For detailed deployment and troubleshooting information, see:
- **[DEPLOYMENT_TRANSLATION_FILES.md](../../../DEPLOYMENT_TRANSLATION_FILES.md)** - Complete deployment guide
- **[SETUP.md](../../../SETUP.md)** - Initial setup and installation guide
