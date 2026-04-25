# 🍳 Cook API (Learn .NET)

โปรเจกต์นี้เป็น API สำหรับจัดการสูตรอาหาร (Recipes) และวัตถุดิบ (Ingredients) สร้างขึ้นเพื่อเป็นแบบฝึกหัดในการเรียนรู้การพัฒนา Backend ด้วย **ASP.NET Core Web API** ร่วมกับฐานข้อมูล **PostgreSQL** โดยใช้ **Entity Framework Core (EF Core)** ในการช่วยจัดการข้อมูล

---

## 🛠️ Tech Stack (เครื่องมือที่ใช้)
- **Framework:** .NET 8 (ASP.NET Core Web API)
- **Database:** PostgreSQL
- **ORM:** Entity Framework Core (EF Core) + Npgsql
- **Tools:** Swagger (สำหรับทดสอบ API)

---

## ✨ Features (ระบบทำอะไรได้บ้าง)
- **Ingredient API (`/api/ingredient`)**
  - ดึงข้อมูลวัตถุดิบทั้งหมด / ดึงตาม ID
  - เพิ่ม แก้ไข และลบวัตถุดิบ
- **Recipe API (`/api/recipe`)**
  - ดึงข้อมูลสูตรอาหาร (รวมลิสต์วัตถุดิบที่ใช้และปริมาณ)
  - เพิ่มสตรอาหารใหม่ **(มีการตรวจสอบว่าวัตถุดิบที่ส่งมาต้องมีในระบบก่อนเท่านั้น)**
  - อัปเดตสูตรอาหาร
  - ลบสูตรอาหาร

---

## 🧠 สิ่งที่ได้เรียนรู้จากโปรเจกต์นี้

### 1. การกำหนดความสัมพันธ์แบบ Many-To-Many ใน EF Core
สูตรอาหาร 1 สูตรใช้วัตถุดิบได้หลายอย่าง และวัตถุดิบ 1 อย่างก็อยู่ในหลายสูตรได้ เราได้เรียนรู้วิธีการสร้าง **Junction Table** (`RecipeIngredient.cs`) ขึ้นมาเชื่อมกลางเพื่อเก็บข้อมูล "ปริมาณที่ใช้" (Quantity) และการตั้งค่าใน `ApiDbContext.cs`:
```csharp
modelBuilder.Entity<RecipeIngredient>()
    .HasKey(ri => new { ri.RecipeId, ri.IngredientId }); // Composite Key
```

### 2. DTO (Data Transfer Object) สำคัญมาก!
จากตอนแรกที่เราพยายามใช้ Database Model มารับข้อมูลจากหน้าบ้าน (Client) ตรงๆ มันทำให้เกิดความซับซ้อนและช่องโหว่ เราเลยเปลี่ยนมาใช้ **DTO (`CreateRecipeDto.cs`)** เพื่อทำการรับข้อมูลที่เข้าใจง่ายแทน แล้วให้หลังบ้านนำ DTO ไปแปลงเป็น Database Model อีกที

### 3. Dependency Injection (DI) ลับหลัง .NET
ได้รู้ว่าเราไม่ต้อง `new ApiDbContext()` เองทุกรอบ แต่เราสามารถเอาใส่ไว้ใน Constructor ของ Controller ได้เลย แล้วเดี๋ยว .NET จะเป็นคน **"ฉีด (Inject)"** ออเจกต์หน้าตาเหมือนกันไปให้ทุกคลาสที่ต้องการใช้งานอัตโนมัติ 

### 4. การจัดการกับ Request ฝั่ง Controller
- การใช้ Attribute เช่น `[ApiController]`, `[HttpPost]`, `[HttpGet]` 
- การใช้ `IActionResult` เพื่อโยนข้อความพร้อม HTTP Status กลับไป (เช่น `Ok(...)` 200, `BadRequest(...)` 400, `NotFound(...)` 404)
- การใช้คำสั่ง `.Include().ThenInclude()` แทนการเชื่อมตารางแบบ `JOIN` ใน SQL หากไม่ใช้ Data ใน List ภายนอกจะออกมาเป็นค่าหลอก (Null/Empty)

### 5. Validate Before Save 
เมื่อก่อนเราอาจจะเขียนแทรกข้อมูลไปเลยรวดเดียว แต่ได้เรียนรู้ว่า **ต้องตรวจสอบความถูกต้องก่อนเสมอ** เช่น การดักจับว่าพบ Ingredient ในระบบหมดหรือยัง? ถ้าไม่พบสัก 1 อัน ให้โยน `400 Bad Request` ออกไปทันที ห้ามลงข้อมูลกึ่งกลางลงตารางเด็ดขาด เพื่อรักษาความสมบูรณ์ของระบบ

---

## 🚀 วิธีการรันโปรเจกต์

1. **เตรียม Database:** ตรวจสอบว่า `appsettings.json` มี Connection String ไปยัง PostgreSQL ของคุณแล้ว
2. **อัปเดต Database Schema:**
   รันคำสั่งนี้เพื่อสร้างตารางผ่าน Migrations ล่าสุด:
   ```bash
   dotnet ef database update
   ```
3. **รันโปรเจกต์:**
   ```bash
   dotnet run
   ```
4. **เปิดดู API / Swagger:**
   ไปที่ `http://localhost:5250/swagger` (หรือ Port ตามที่พิมพ์บน Terminal) เพื่อทดสอบ API ได้ง่ายๆ ผ่านหน้าเว็บเลย!
