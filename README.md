# نظام إدارة صيانة الأجهزة والشركات (ASP.NET Core MVC - .NET 10)

## 1. تثبيت الباكدجات
الباكدجات متسجلة في الـcsproj بالفعل، تشغل:
```
dotnet restore
```

## 2. الـConnection String
في appsettings.json:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=MaintenanceSystemDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

## 3. عمل الـMigration وتشغيل المشروع
```
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run
```

عند أول تشغيل، DbSeeder بيعمل تلقائي:
- الـ4 Roles: Admin, ITManager, Technician, Employee
- أدمن افتراضي: admin@maintenance.local / Admin@12345

## 4. الترتيب الصحيح لتجهيز البيانات بعد أول تشغيل
1. سجل دخول بالأدمن
2. من "الشركات" أضف شركة
3. من "الأقسام" أضف قسم تابع للشركة
4. من "المستخدمين" أضف موظفين/فنيين واختار لهم قسم
5. من "الأجهزة" أضف جهاز واربطه بقسم وموظف صاحب الجهاز
6. (اختياري) من "SLA" حدد مهلة الاستجابة/الحل لكل أولوية
7. (اختياري) من "قطع الغيار" جهز مخزون القطع

بعد كده أي موظف يقدر يفتح تذكرة صيانة ويختار الجهاز، والنظام هيحدد القسم وصاحب الجهاز تلقائي.

## 5. تدفق العمل (Workflow)
Employee يفتح تذكرة (Pending) → ITManager/Admin يعين فني (تتحول IN Progress) → الفني يشتغل ويغير الحالة → Resolved → الموظف يأكد الإغلاق → Closed.
كل تغيير حالة بيتسجل في StatusHistory، وبيتبعت إشعار للموظف.

## 6. الصلاحيات (Roles)
- **Admin**: كل الصلاحيات + إدارة المستخدمين والبيانات الأساسية
- **ITManager**: يشوف كل التذاكر، يعين فنيين، يشوف التقارير والداشبورد
- **Technician**: يشوف التذاكر المعينة له، يغير الحالة، يضيف قطع غيار وتعليقات
- **Employee**: يفتح تذاكر، يشوف تذاكره بس، يأكد إغلاقها بعد الحل

## 7. الملفات الأساسية
- `Data/ApplicationDbContext.cs` - كل العلاقات بين الجداول
- `Data/DbSeeder.cs` - الأدوار والأدمن الافتراضي
- `Controllers/TicketsController.cs` - قلب النظام (فتح/تعيين/تغيير حالة/تعليقات/مرفقات/قطع غيار)
- `Controllers/DashboardController.cs` - الأرقام (Total, Open, In Progress, Resolved, Overdue)
- `Controllers/ReportsController.cs` - أداء الفنيين
- `Controllers/AdminController.cs` - المستخدمين + الشركات + الأقسام + الأجهزة + قطع الغيار + SLA

## 8. حاجات ممكن تتضاف لاحقًا
- رفع مرفقات فعلي في صفحة تفاصيل التذكرة (الكود جاهز في الكونترولر، محتاج فورم في الـView)
- Job دوري يحدث IsOverdue تلقائي بدل ما يتحسب وقت العرض بس
- صفحة Edit/Delete للأجهزة والأقسام (دلوقتي بس Create + List)
