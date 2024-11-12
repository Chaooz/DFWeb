!/bin/bash
#
# Script to start the post process of CSS for tailwind
#
npx tailwindcss -c ./wwwroot/lib/tailwind/tailwind.config.js -i ./wwwroot/lib/tailwind/tailwind_input.css -o ./wwwroot/css/tailwind_output.css --watch