#  Ghid de Dezvoltare și Colaborare

Bine ați venit în echipa de dezvoltare SideQuest! Pentru a menține un flux de lucru profesionist și un cod stabil, vă rugăm să respectați următoarele standarde de utilizare a repository-ului.

---

### 1. Stabilitatea Branch-ului Principal (`main`)
* **Interzis lucrul direct pe `main`.** Acest branch este rezervat exclusiv versiunilor stabile de cod.
* **NU** efectuați niciodată `push` direct pe `main`.
* Codul aflat pe acest branch trebuie să fie funcțional și capabil de rulare (build) în orice moment.

### 2. Fluxul de Lucru (Feature Branching)
Înainte de a începe orice task, creați un branch nou plecând de la ultima versiune de `main`:
```bash
git checkout main
git pull origin main
git checkout -b feature/nume-task
```
*Exemple: `feature/user-profile`, `feature/sqlite-config`*

### 3. Sincronizarea Înainte de lucru (Evitarea Conflictelor)
Pentru a preveni erorile de tip "merge conflict", sincronizați-vă branch-ul local cu munca echipei la începutul fiecărei sesiuni:
```bash
git checkout main
git pull origin main
git checkout feature/branch-ul-tau
git merge main
```

### 4. Standarde pentru Commit-uri
* **Atomic Commits:** Trimiteți modificări mici și logice, nu seturi masive de fișiere deodată.
* **Mesaje Descriptive:** Folosiți prefixe standard pentru a identifica tipul modificării:
    * `feat:` (funcționalitate nouă)
    * `fix:` (remediere bug)
    * `docs:` (modificări documentație)
    * ✅ *Exemplu: `feat: adaugat validare varsta in UserProfile`*

### 5. Pull Requests (PR) și Code Review
* După finalizarea unui task, urcați branch-ul pe GitHub și deschideți un **Pull Request**.
* **Peer Review:** Cel puțin un alt membru (Paul sau Alex Bonațiu) al echipei trebuie să revizuiască și să aprobe codul înainte ca acesta să fie integrat (merged) în branch-ul `main`.

### 6. Arhitectura N-Tier (Separarea Responsabilităților)
Respectați cu strictețe separarea straturilor proiectului:
* **SideQuest.UI:** Doar pagini Blazor și logică de interfață.
* **SideQuest.BLL:** Logica de business, modelele de date și validările.
* **SideQuest.DAL:** Contextul bazei de date, migrări și repository-uri.

### 7. Securitate și Autentificare
GitHub nu mai acceptă parola contului pentru operațiuni în terminal. Folosiți un **Personal Access Token (PAT)** sau **GitHub CLI** (`gh auth login`) pentru autentificare.

---
*Succes tuturor!*