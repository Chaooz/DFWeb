@echo off
REM 
REM Script to start the post process of CSS for tailwind
REM
npx tailwindcss -i ./wwwroot/lib/tailwind/tailwind_input.css -o ./wwwroot/lib/css/tailwind_output.css --watch