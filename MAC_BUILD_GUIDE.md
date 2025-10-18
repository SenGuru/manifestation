# Complete Mac Build & Submission Guide for Mistel

## Prerequisites

Before you start, make sure you have:
- [ ] MacBook with macOS (any recent version)
- [ ] Apple Developer Account ($99/year) - Sign up at https://developer.apple.com
- [ ] Your project files transferred to Mac
- [ ] 2-4 hours of free time

---

## Step 1: Install Required Software

### 1.1 Install Xcode (Required)

1. Open **App Store** on your Mac
2. Search for "Xcode"
3. Click **GET** / **Install** (it's FREE but ~12GB download)
4. Wait for installation (can take 30-60 minutes)
5. Open Xcode once to accept license agreement

**Verify Installation:**
```bash
xcode-select --install
```

### 1.2 Install Flutter (If not already installed)

1. Download Flutter SDK: https://flutter.dev/docs/get-started/install/macos
2. Extract to a location like `~/development/flutter`
3. Add to PATH:
```bash
export PATH="$PATH:$HOME/development/flutter/bin"
```

**Verify Installation:**
```bash
flutter doctor
```

Look for green checkmarks ✓ for Flutter and Xcode.

### 1.3 Install CocoaPods (Required for iOS)

```bash
sudo gem install cocoapods
```

---

## Step 2: Transfer Project to Mac

### Option A: If using Git
```bash
cd ~/Documents
git clone [your-repo-url]
cd manifestation/source/mobile
```

### Option B: USB Transfer / Cloud Drive
1. Copy entire project folder to Mac
2. Open Terminal
3. Navigate to project:
```bash
cd ~/Documents/manifestation/source/mobile
```

---

## Step 3: Initial Flutter Setup

In Terminal, inside your project directory:

```bash
# Get Flutter dependencies
flutter pub get

# Clean any previous builds
flutter clean

# Install iOS pods
cd ios
pod install
cd ..

# Verify everything works
flutter doctor -v
```

**Expected output:** Should see checkmarks for Flutter, Xcode, and iOS toolchain.

---

## Step 4: Configure iOS Bundle Identifier

### 4.1 Choose Your Bundle ID
Format: `com.yourcompany.mistel`

Example: `com.senthil.mistel`

**This must be UNIQUE across all apps in the App Store!**

### 4.2 Update in Xcode

1. Open Terminal in project directory:
```bash
cd ios
open Runner.xcworkspace
```

2. Wait for Xcode to load (first time can take a minute)

3. In Xcode left sidebar, click on **Runner** (blue icon at top)

4. Under "TARGETS", select **Runner**

5. Click **Signing & Capabilities** tab

6. **Automatically manage signing**: Check this box ✓

7. **Team**: Select your Apple Developer account
   - If you don't see your team, click "Add Account..."
   - Sign in with your Apple ID

8. **Bundle Identifier**: Change to your unique ID
   - Example: `com.senthil.mistel`

9. You should see "✓ Signing for 'Runner' requires a development team..."

---

## Step 5: Update App Display Name & Version

Still in Xcode:

1. Go to **General** tab (not Signing & Capabilities)

2. **Display Name**: Change to "Mistel"

3. **Version**: Set to "1.0"

4. **Build**: Set to "1" (increment this for each submission)

5. Press **Cmd+S** to save

---

## Step 6: Configure App Icon

### 6.1 Prepare App Icon

You need a 1024x1024 PNG image with NO transparency.

### 6.2 Add to Xcode

1. In Xcode left sidebar, navigate to:
   `Runner > Assets.xcassets > AppIcon`

2. Drag your 1024x1024 PNG to the **App Store iOS** slot

3. Xcode will automatically generate all required sizes

---

## Step 7: (IMPORTANT) Replace QA URLs with Production

### 7.1 Update API Base URL

Open `lib/services/api_client.dart` and change line 6:

**FROM:**
```dart
static const String baseUrl = 'http://manifestation-qa-1.senthilguru.net';
```

**TO:**
```dart
static const String baseUrl = 'https://api.mistel.app'; // YOUR PRODUCTION HTTPS URL
```

**⚠️ CRITICAL: Must be HTTPS (not HTTP) for App Store approval!**

### 7.2 Update Any Other URLs

Search for other QA references:
```bash
grep -r "qa" lib/
grep -r "senthilguru.net" lib/
```

Replace any QA/test URLs with production HTTPS URLs.

---

## Step 8: Build for iOS Device

### 8.1 Select Build Configuration

1. In Xcode, top bar, click the device dropdown (next to "Runner")
2. Select **Any iOS Device (arm64)**

### 8.2 Archive the App

1. In Xcode menu: **Product > Archive**
2. Wait for build to complete (5-10 minutes first time)
3. If successful, Archive Organizer window will open

**Common Build Errors:**

**Error: "Signing for Runner requires a development team"**
- Fix: Go to Signing & Capabilities, select your team

**Error: "Provisioning profile doesn't include signing certificate"**
- Fix: In Xcode Preferences > Accounts, download certificates

**Error: "Command PhaseScriptExecution failed"**
- Fix: Run `flutter clean && flutter pub get` then try again

---

## Step 9: Upload to App Store Connect

### 9.1 Create App in App Store Connect

1. Go to https://appstoreconnect.apple.com
2. Click **My Apps**
3. Click **+** button (top left) > **New App**
4. Fill in:
   - **Platforms**: iOS
   - **Name**: Mistel (must be available)
   - **Primary Language**: English (U.S.)
   - **Bundle ID**: Select the one you created (com.yourcompany.mistel)
   - **SKU**: mistel-ios-1 (any unique identifier for your records)
   - **User Access**: Full Access
5. Click **Create**

### 9.2 Upload Build from Xcode

1. In Archive Organizer (after successful archive):
   - Select your archive
   - Click **Distribute App** button

2. Select **App Store Connect** > Next

3. Select **Upload** > Next

4. Select **Automatically manage signing** > Next

5. Review options (defaults are fine) > Next

6. Click **Upload**

7. Wait for upload (5-15 minutes depending on internet)

8. When complete, you'll see "Upload Successful"

---

## Step 10: Fill in App Store Connect Metadata

Now back in App Store Connect (https://appstoreconnect.apple.com):

### 10.1 App Information

1. Go to your app > **App Information**

2. Fill in:
   - **Subtitle**: "Manifest Your Dream Life" (30 char max)
   - **Privacy Policy URL**: https://[your-url]/privacy
   - **Category**:
     - Primary: Health & Fitness
     - Secondary: Lifestyle

3. Click **Save**

### 10.2 Pricing and Availability

1. Click **Pricing and Availability** (left sidebar)
2. Select your price tier (Free or paid)
3. Select countries/regions (or "All Countries")
4. Click **Save**

### 10.3 App Privacy

1. Click **App Privacy** (left sidebar)
2. Click **Get Started**
3. Answer questions about data collection:

**Data Collection:**
- ✓ Yes, we collect data from this app

**Data Types:**
- Contact Info: Email Address (for account creation)
- User Content: Photos, Audio, Other User Content
- Identifiers: User ID
- Usage Data: Product Interaction, App Interactions

4. For each data type, specify:
   - **Used for**: App Functionality, Analytics
   - **Linked to user**: Yes
   - **Used for tracking**: No

5. Click **Publish**

### 10.4 Version Information

1. Click on your app version (e.g., "1.0 - Prepare for Submission")

2. **Screenshots** (REQUIRED):
   - Upload 3-10 screenshots for iPhone 6.7" display
   - Upload 3-10 screenshots for iPhone 6.5" display
   - You can use placeholders for now, create them later

3. **Description**: Copy from `APP_STORE_METADATA.md`

4. **Keywords**: manifestation,meditation,affirmations,vision board,goals

5. **Support URL**: https://[your-support-url]

6. **Marketing URL**: https://mistel.app (optional)

7. **What's New in This Version**:
   ```
   Initial release of Mistel!

   Features:
   • Daily affirmations with voice narration
   • Vision board creator
   • Goal tracking
   • Manifestation journal
   • Visual meditation
   • Usage analytics
   ```

### 10.5 Build Selection

1. Scroll to **Build** section
2. Click **+** next to "Build"
3. Wait a few minutes if build doesn't show (processing time)
4. Select your uploaded build
5. Click **Done**

### 10.6 General Information

1. **App Icon**: Should auto-populate from build
2. **Version**: 1.0
3. **Copyright**: 2025 [Your Name/Company]
4. **Age Rating**:
   - Click **Edit**
   - Answer questions (all "No" for Mistel)
   - Should result in **4+**

### 10.7 App Review Information

**Contact Information:**
- First Name: [Your First Name]
- Last Name: [Your Last Name]
- Phone: [Your Phone]
- Email: [Your Email]

**Sign-In Required:**
If your app requires login:
- ✓ Sign-in required
- Username: demo@mistel.app
- Password: [your demo password]

**Notes for Review:**
```
Mistel is a manifestation and meditation app. All features work as expected.

Test account credentials:
Email: demo@mistel.app
Password: [demo password]

How to test:
1. Log in with test account
2. Try Daily Affirmations with voice playback
3. Create a vision board (tap + button, add image/text)
4. Create a goal in Goal Tracker
5. Write a journal entry
6. Start a meditation session

Thank you!
```

---

## Step 11: Submit for Review

1. Review everything one more time
2. Scroll to top and click **Add for Review**
3. Select **Manually release this version** (recommended for first version)
4. Check **Export Compliance**:
   - "Is your app designed to use cryptography or does it contain or incorporate cryptography?"
   - Select **No** (we only use standard HTTPS)
5. Click **Submit for Review**

🎉 **Done!** Your app is now in review.

---

## Step 12: Wait for Review

**Timeline:**
- Average: 1-2 days
- Can be 24 hours to 1 week
- You'll get email updates

**Possible Outcomes:**

### ✅ Approved
- You'll receive "Ready for Sale" email
- App goes live automatically (or manually if you chose that)
- 🎉 Congratulations!

### ❌ Rejected
Don't panic! This is normal. Common reasons:
- Missing information
- Bug or crash during testing
- Guideline violations
- Privacy policy issues

**If rejected:**
1. Read rejection email carefully
2. Fix the issues
3. Increment build number in Xcode
4. Create new archive
5. Upload new build
6. Resubmit

---

## Common Issues & Solutions

### "Command PhaseScriptExecution failed with a nonzero exit code"
```bash
cd ios
pod deintegrate
pod install
cd ..
flutter clean
flutter pub get
```
Then try building again.

### "Flutter.framework not found"
```bash
flutter clean
flutter pub get
```

### "Signing certificate invalid"
1. Xcode > Preferences > Accounts
2. Select your Apple ID
3. Download Manual Profiles
4. Try again

### "No provisioning profiles with a valid signing identity"
1. Make sure "Automatically manage signing" is checked
2. Select correct Team
3. Clean build folder: Product > Clean Build Folder
4. Try again

### Build succeeds but upload fails
- Check your internet connection
- Try uploading again (sometimes Apple's servers have issues)
- Wait 10 minutes and retry

---

## Quick Command Reference

```bash
# Navigate to iOS directory
cd ios

# Install pods
pod install

# Clean everything
flutter clean
cd ios && pod deintegrate && pod install && cd ..

# Check Flutter setup
flutter doctor -v

# Build iOS (command line alternative to Xcode)
flutter build ios --release

# Open in Xcode
open ios/Runner.xcworkspace
```

---

## Checklist - Do in Order

### Before Opening Xcode:
- [ ] Project transferred to Mac
- [ ] Xcode installed
- [ ] Flutter installed
- [ ] CocoaPods installed
- [ ] `flutter pub get` run successfully
- [ ] `pod install` completed

### In Xcode:
- [ ] Opened `Runner.xcworkspace` (NOT Runner.xcodeproj!)
- [ ] Selected Team in Signing & Capabilities
- [ ] Set unique Bundle Identifier
- [ ] Enabled "Automatically manage signing"
- [ ] Set Display Name to "Mistel"
- [ ] Set Version to 1.0
- [ ] Added 1024x1024 app icon
- [ ] Replaced QA URLs with HTTPS production URLs
- [ ] Selected "Any iOS Device" as build target
- [ ] Product > Archive succeeded
- [ ] Distributed to App Store Connect
- [ ] Upload successful

### In App Store Connect:
- [ ] Apple Developer account active
- [ ] New app created
- [ ] App name available
- [ ] Privacy policy URL added
- [ ] Screenshots uploaded (at least 3 per size)
- [ ] Description filled
- [ ] Keywords added
- [ ] Support URL added
- [ ] Build selected
- [ ] Age rating set (4+)
- [ ] App Review info filled
- [ ] Demo account credentials provided
- [ ] Export compliance answered
- [ ] Submitted for review

---

## Need Help?

- Apple Developer Forums: https://developer.apple.com/forums/
- Flutter Discord: https://discord.gg/flutter
- Stack Overflow: Tag questions with `ios`, `flutter`, `xcode`

---

## After Approval

Once approved:
1. App goes live on App Store!
2. Share with friends/family
3. Start marketing
4. Monitor reviews
5. Plan updates and new features

**Congratulations on publishing your iOS app!** 🎉📱
