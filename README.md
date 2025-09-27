
# ☕ Coffee Demo – Gameplay Prototype

## 📌 Overview

This Unity project is a demo created as part of an interview task.
The game simulates running a small coffee cart where the player must:

1. Collect coffee bean bags.
2. Process them into cups of coffee using a machine.
3. Deliver coffee to customers.
4. Earn rewards for successful deliveries.

---

## 🎮 Core Features

* **Player Controller**

  * Simple joystick / WASD movement
  * Confined to a small play area
  * Highlight feedback:

    * 🟢 Green → Cup in range
    * 🟡 Yellow → Machine / Resource in range
    * ⚪ White → Nothing nearby

* **Resource Area**

  * Infinite coffee bean supply
  * Stack up to **3 bags** on the player’s back

* **Coffee Machine**

  * Accepts one bag per interaction
  * Short processing timer
  * Spawns a cup of coffee on completion

* **Cup Pickup & Delivery**

  * Player can hold only **one cup at a time**
  * Deliver to customers

* **Customer AI**

  * Customers spawn at serving counter
  * Disappear after being served
  * New customers spawn automatically
  * Exit Point defined for walk-out

* **Rewards**

  * On delivery, coins fly from customer to the UI score

---

## 📱 Controls

* **PC (Editor / Standalone)**

  * Move: `WASD` / Arrow Keys
  * Interact: `E`

* **Mobile (APK)**

  * On-screen joystick for movement
  * On-screen button for interaction

---

## 🛠️ Tech Details

* Engine: **Unity 2021/2022 LTS**
* Language: **C#**
* Scripting Backend: **IL2CPP**
* Target Platforms: **Android (APK)**, PC (Editor testing)

---

## 📂 Project Structure

```
Assets/
  Scripts/         # Core gameplay scripts
  Prefabs/         # Player, Customer, Coffee Machine, Resource Area
  UI/              # Basic UI (score, interact button)
Packages/
ProjectSettings/
```

---

## ▶️ How to Play

1. Move to the **Resource Area** → collect up to 3 bags.
2. Take bags to the **Coffee Machine** → interact to process.
3. Pick up the **Cup of Coffee** when ready.
4. Deliver cup to the **Customer** at the counter.
5. Customer exits, coins fly to UI, new customer spawns.

---

## 📲 Build

* **Platform:** Android (APK)
* **Minimum API:** Android 10.0 (API 24)
* **Target API:** Android 13/14/15 (33/34/35)
* **Architectures:** ARMv7, ARM64

---

## 👨‍💻 Author

Developed by **\suvaneethan** as part of an interview technical task.

---

