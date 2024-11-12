REM 
REM Script to install Tailwind CSS > https://tailwindcss.com/docs/installation
REM NPM Requires Node.js : https://nodejs.org/
REM

npm install -D tailwindcss
npx tailwindcss init

REM
REM Edit config tailwind.config.js 
REM Add 
REM /** @type {import('tailwindcss').Config} */
REM module.exports = {
REM   content: ["./src/**/*.{html,js}"],
REM   theme: {
REM     extend: {},
REM   },
REM   plugins: [],
REM }
REM
