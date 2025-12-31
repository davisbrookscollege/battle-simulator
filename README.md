# Battle Simulator 
Battle Simulator is a simple full‑stack project with a C# backend and a React frontend.

It is capable of:
* Simulating battles between Transformers
* Displaying information about Transformers
* Creating and deleting Transformers

## How to Run the Project

### 1. Start the backend (API)
```bash
cd battlesimulation.api
dotnet restore
dotnet run
```
The API will start on the local HTTPS port https://localhost:7297

### 2. Start the frontend (UI)
```bash
cd battlesimulation-ui
npm install
npm run dev
```
The React app will start on the local HTTPS port http://localhost:3000

### 3. Use the App
* Open the frontend in your browser
* The UI will automatically call the backend
* Enter fighters and run battles

Created by Davis Brooks :)
