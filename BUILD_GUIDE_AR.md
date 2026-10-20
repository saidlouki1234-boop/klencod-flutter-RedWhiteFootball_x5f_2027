
# دليل البناء السريع

1. افتح Unity Hub 2022.3.20f1
2. Open مشروع RedWhiteFootball_2027
3. Window > Package Manager > تأكد من TextMeshPro مثبت
4. افتح Scenes/MainMenu ثم File > Build Settings > Add Open Scenes > أضف MainMenu و Match
5. Switch to Android
6. Edit > Project Settings > XR Plug-in Management > عطّل AR
7. Build APK

## حل مشكلة Joystick
FixedJoystick موجود في TouchController.cs - اسحب background/handle من UI Canvas. يمكنك إنشاء Canvas بسيط:
- Canvas (Screen Space Overlay)
- Panel Joystick (يسار) > Image background > Image handle
- Buttons: Pass, Shoot, Sprint (يمين) واربطها بدوال OnPassClick, OnShootDown/Up, OnSprintDown/Up في TouchController

الواجهة مصممة بألوان: أحمر #D10A1F وأبيض #FFFFFF وأخضر ملعب #2E7D32
استخدم خط Cairo أو Tajawal لدعم العربية في TextMeshPro.
