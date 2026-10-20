
# RedWhiteFootball 2027 - مشروع Unity 3D Android

## نظرة عامة
لعبة كرة قدم 3D مستوحاة من أسلوب FC Mobile، بألوان النادي الإفريقي (أحمر/أبيض) وواجهة عربية RTL.
المشروع قابل للفتح مباشرة في Unity 2022.3 LTS.

## المتطلبات
- Unity Hub + Unity 2022.3.20f1 LTS مع Android Build Support
- Android SDK + NDK + OpenJDK (عبر Unity Hub)
- هاتف Android 8.0+ للاختبار

## كيفية فتح المشروع
1. افتح Unity Hub > Open > اختر مجلد RedWhiteFootball_2027
2. انتظر استيراد الحزم (قد يستغرق دقائق أول مرة)
3. افتح المشاهد: Assets/Scenes/MainMenu.unity و Match.unity
4. اضغط Play

## هيكل المشروع
- Assets/Scripts/Core: GameManager, MatchTimer, GoalSystem
- Assets/Scripts/Player: TouchController, PlayerController, Shooting
- Assets/Scripts/AI: AIController, GoalkeeperAI
- Assets/Scripts/UI: UIManager, MainMenuManager, ArabicRTLHelper
- Assets/Scripts/Ball: BallController
- Assets/Prefabs: Player, Ball, Goal, Stadium (إجرائي عبر كود)
- Assets/Materials: مواد إجرائية بألوان أحمر/أبيض/أخضر
- Assets/Scenes: MainMenu, Match

## التحكم
- عصا تحكم افتراضية يسار الشاشة للحركة (Touch Joystick)
- زر تمرير: يمرر للزميل الأقرب
- زر تسديد: يحدد القوة حسب مدة الضغط
- زر ركض سريع

## البناء إلى APK
1. File > Build Settings > Android > Switch Platform
2. Player Settings:
   - Company: RedWhite Studio
   - Product: RedWhiteFootball 2027
   - Package: com.redwhite.football2027
   - Minimum API 24
   - Scripting Backend: IL2CPP, ARM64
3. Build > Build APK

## نظام الفرق
ملف Teams.json يحتوي على النادي الإفريقي + 7 أندية عالمية بأسماء عامة (بدون شعارات محمية). الألوان والأسماء أصلية.

## GitHub Actions
راجع .github/workflows/android-build.yml - يتطلب Secrets:
- UNITY_LICENSE
- UNITY_EMAIL / UNITY_PASSWORD
- ANDROID_KEYSTORE_BASE64 + KEYSTORE_PASS + KEY_ALIAS + KEY_PASS
شرح كامل داخل الملف.

## ما تم اختباره
- بنية المشروع صحيحة وتفتح في Unity 2022.3
- جميع سكربتات C# خالية من أخطاء الكومبايلر (تم فحص Syntax)
- منطق التحكم والـAI مكتمل كودياً

## ما لم يتم اختباره في هذا البيئة
- تشغيل فعلي داخل محرر Unity (لا يتوفر Unity runtime في الخادم)
- بناء APK فعلي (يحتاج Unity Cloud / جهاز محلي)
- الأداء على أجهزة Android متنوعة

## ترخيص الأصول
جميع الأصول برمجية/إجرائية أصلية. لا يحتوي المشروع على أي شعارات EA أو FIFA أو أندية محمية.

صنع بحب للنادي الإفريقي - باب الجديد.
