# Ghid de Dezvoltare și Colaborare

Bine ați venit în echipa de dezvoltare SideQuest! Pentru a menține un flux de lucru profesionist și un cod stabil, vă rugăm să respectați următoarele standarde de utilizare a repository-ului.
 
---

### 1. Structura Branch-urilor
Folosim două branch-uri protejate:
* **`main`** — doar versiuni stabile. Nu lucrați niciodată direct aici.
* **`dev`** — branch-ul activ de dezvoltare. Aici se integrează toată munca echipei.
  Ambele branch-uri sunt protejate. Nu este permis push direct — totul trece printr-un Pull Request.

### 2. Fluxul de Lucru (Feature Branching)
Înainte de a începe orice task, creați un branch nou plecând de la ultima versiune a `dev`:
```bash
git checkout dev
git pull origin dev
git checkout -b feature:nume-task
```
*Exemple: `feature:user-profile`, `feature:event-model`, `feature:map-api`*

### 3. Standarde pentru Commit-uri
* **Commit-uri Atomice:** Trimiteți modificări mici și logice — nu seturi masive de fișiere deodată.
* **Mesaje Descriptive:** Folosiți prefixe standard pentru a identifica tipul modificării:
  * `feat:` — funcționalitate nouă
  * `fix:` — remediere bug
  * `docs:` — modificări documentație
  * `refactor:` — restructurare cod fără schimbarea comportamentului
  * ✅ *Exemplu: `feat: adaugat validare varsta in UserProfile`*
### 4. Pull Requests și Code Review
* După finalizarea unui task, urcați branch-ul pe GitHub și deschideți un **Pull Request** către `dev`.
* Cel puțin un alt membru al echipei trebuie să revizuiască și să aprobe codul înainte de integrare.
* Merge-urile `dev` → `main` au loc doar la milestone-uri stabile și necesită același proces de aprobare.
### 5. Structura Proiectului (Separarea Responsabilităților)
Respectați cu strictețe separarea straturilor:
* **SideQuest.UI** *(React Native)* — ecrane, componente, navigație și logică de interfață.
* **SideQuest.BLL** — logica de business, modelele de date și validările.
* **SideQuest.DAL** — contextul bazei de date, migrări și repository-uri.
* **SideQuest.TEST** — folderul pentru testarea funcționalității.
  Nu referențiați `SideQuest.UI` din `BLL` sau `DAL`. Dependențele circulă doar în jos.
