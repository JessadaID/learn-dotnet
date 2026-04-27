# Learn .NET Core API สู่การทำ JWT แบบเจาะลึก

โปรเจกต์นี้เป็นการทดลองและเรียนรู้เกี่ยวกับการสร้าง Web API เบื้องต้นด้วยภาษา C# และ ASP.NET Core โดยโฟกัสไปที่ระบบการจัดการข้อมูล (CRUD) ร่วมกับฐานข้อมูล PostgreSQL ผ่าน Entity Framework Core และขยับไปสู่ระบบ Authentication ด้วย **JWT (JSON Web Token)**

## ทำอะไรไปบ้างแล้วในโปรเจกต์นี้?
1. **ออกแบบและสร้าง Web API พื้นฐาน:**
   - สร้าง Controller `TodosController` สำหรับดึงและเพิ่มข้อมูลผ่าน HttpGet และ HttpPost
2. **ต่อฐานข้อมูล PostgreSQL ผ่าน EF Core:**
   - เขียน Schema หรือ `Models` สำหรับ `Todo` และ `User`
   - ตั้งค่า `AppDbContext` ในลักษณะ Code-First และใช้คำสั่ง Migration (`dotnet ef migrations add`, `dotnet ef database update`) เพื่อสร้างตาราง
3. **ระบบสมัครสมาชิกและเข้าสู่ระบบ (Authentication):**
   - สร้าง `AuthController` และจำลอง Logic ล็อกอิน หาข้อมูลใน DB โดยการเช็ค Username / Password
   - เขียนคลาส `TokenService` ที่บรรจุ 5 ขั้นตอนสำคัญในการสร้าง JWT (การเตรียม Claims, การเลือก Algorithm HmacSha256/512, ใช้ Secret Key)
4. **จัดการสิทธิ์การเข้าถึงข้อมูล (Authorization):**
   - การใช้ `[Authorize]` เพื่อป้องกันสิทธิ์คนนอกเข้า API
   - การปลดข้อจำกัดชั่วคราวให้เข้าแบบไม่ต้องมีบัตรด้วย `[AllowAnonymous]`
   - การแบ่งชนชั้นตำแหน่งด้วยออปชันการเช็ค Roles `[Authorize(Roles = "admin, user")]`

##  สิ่งที่ได้เรียนรู้จากโปรเจกต์นี้
- **ข้อควรรู้ของ Non-Nullable Property:** หากเราไม่ใส่ `?` บน property ของโมเดล C# จะบังคับ (Required) ข้อมูลนั้นทันทีเวลาที่รับผ่าน JSON `[FromBody]`
- **การตั้งชื่อตัวแปรห้ามซ้ำซ้อน (Variable Shadowing):** เรียนรู้วิธีการแก้บัคเมื่อตัวแปรที่ดึงจาก DB มีชื่อซ้ำกับ Parameter ในฟังก์ชัน เช่น `var user`
- **ปัญหารหัสผ่าน (Secret Key) ของ JWT:** ต้องมีความยาวขั้นต่ำตามขนาดขอ Algorithm หากเลือก `HmacSha512` ตัวหนังสือกุญแจลับที่เก็บในเครื่องก็ต้องยาวมากกว่า 64 ตัวอักษรขึ้นไป
- **ความสะดวกของ EF Core:** `Id` จะถูกนึกว่าเป็น PK และ Auto-increment ไปให้เอง และเราสามารถใช้ `[Index(nameof(Username), IsUnique = true)]` เพื่อไม่ให้ฐานข้อมูลมี Username ซ้ำกันได้อย่างง่ายดาย

---
*Created by Jessada - Road to Backend Developer *
