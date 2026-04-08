# Tailwindcss forklaring

## Oppstart:
Jeg har installert og lagt inn det som trengs av import filer i dev og vite.config.js filen.
For å bruke Tailwind må det ligge en import i .css filen. html og .tsx filene viser til. Trenger i teorien ikke flere .css filer.
```css
@import "tailwindcss";
```

I html/typescript filene vises det til .css filen på vanlig vis.

I html:
```html
  <link href="/src/style.css" rel="stylesheet">
```
I tsx:
```tsx
 import './index.css'
```
For å kjøre programmet første gang må en kjøre:
 ```terminal
 npm run dev 
 ```



## Bruk:
### Eksempel Tailwind‑kode for layout

```tsx
<div className="h-screen flex items-center justify-center bg-gray-100">
  <div className="p-8 bg-white rounded-xl shadow-xl">
    <h2 className="text-xl text-blue-900 font-bold mb-2">
      Velkommen!
    </h2>
    <p>Utforsk vårt store utvalg av brettspill og ha det gøy med venner.</p>
  </div>
</div>
```

### Cheatsheet

På denne siden ligger navnene på de mest brukte elementene fra css/tailwind, og viser hva kommandoen i tailwind er for hver ønsker css resultat. Legges inn bak "className=" som vist ovenfor.
https://www.webdevultra.com/articles/tailwindcss-cheatsheet-css-equivalents

## Mal:
Her skal jeg legge inn forslag til farger/str og ferdige classeoppsett for enklere og raskere videre bruk.