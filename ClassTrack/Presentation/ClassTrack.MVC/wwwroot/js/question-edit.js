const $ = (s) => document.querySelector(s);

let options = []; 

function setType(type) {

    $("#questionType").value = type;

    const isChoice = type === "choice";
    $("#choiceWrap").style.display = isChoice ? "block" : "none";

    $("#btnTypeChoice").classList.toggle("active", isChoice);
    $("#btnTypeOpen").classList.toggle("active", !isChoice);

    $("#typePill").textContent = isChoice ? "CHOICE QUESTION" : "OPEN QUESTION";
}

function renderOptions() {
    const list = $("#optionsList");
    list.innerHTML = "";

    if (options.length === 0) {
        list.innerHTML = `<div class="muted">No options yet. Click “Add option”.</div>`;
        return;
    }

    const isMultiple = $("#isMultiple").checked;

    options.forEach((opt, idx) => {
        const row = document.createElement("div");
        row.className = "opt";

        row.innerHTML = `
      <div class="field" style="gap:6px;">
        <label class="label">Variant</label>
        <input class="input" data-idx="${idx}" data-field="Variant" value="${escapeHtml(opt.Variant)}" placeholder="e.g. A) ..." />
      </div>

      <div class="field" style="gap:6px;">
        <label class="label">IsCorrect</label>
        <label class="check">
          <input class="opt-correct" type="${isMultiple ? "checkbox" : "radio"}" name="correctOne"
                 data-idx="${idx}" ${opt.IsCorrect ? "checked" : ""} />
          Correct
        </label>
      </div>

      <div class="opt-actions">
        <button class="btn danger" type="button" data-action="remove" data-idx="${idx}">Remove</button>
      </div>
    `;

      
        row.querySelector('input[data-field="Variant"]').addEventListener("input", (e) => {
            options[idx].Variant = e.target.value;
        });

     
        row.querySelector(".opt-correct").addEventListener("change", (e) => {
            if (!isMultiple) {
               
                options = options.map((x, i) => ({ ...x, IsCorrect: i === idx }));
            } else {
                options[idx].IsCorrect = e.target.checked;
            }
            renderOptions(); 
        });

    
        row.querySelector('[data-action="remove"]').addEventListener("click", () => {
            options.splice(idx, 1);
            renderOptions();
        });

        list.appendChild(row);
    });
}

function addOption() {
    options.push({ Variant: "", IsCorrect: false });
    renderOptions();
}

function clearOptions() {
    options = [];
    renderOptions();
}

function buildDTO() {
    const type = $("#questionType").value;

    const base = {
        Title: $("#qTitle").value.trim(),
        Point: Number($("#qPoint").value || 0),
        QuizId: toNullableLong($("#quizId").value)
    };

    if (type === "open") {
        
        return base;
    }

   
    return {
        ...base,
        IsMultiple: $("#isMultiple").checked,
        Options: options.map(o => ({
            Variant: (o.Variant || "").trim(),
            IsCorrect: !!o.IsCorrect
        }))
    };
}

function showJson(obj) {
    const box = $("#jsonBox");
    box.style.display = "block";
    box.textContent = JSON.stringify(obj, null, 2);
}

function toNullableLong(v) {
    const s = String(v ?? "").trim();
    if (!s) return null;
    const n = Number(s);
    return Number.isFinite(n) ? n : null;
}

function escapeHtml(str) {
    return String(str ?? "")
        .replaceAll("&", "&amp;")
        .replaceAll("<", "&lt;")
        .replaceAll(">", "&gt;")
        .replaceAll('"', "&quot;")
        .replaceAll("'", "&#039;");
}


document.addEventListener("DOMContentLoaded", () => {

    setType("choice");
    $("#typePill").textContent = "CHOICE QUESTION";

 

    $("#btnTypeChoice").addEventListener("click", () => setType("choice"));
    $("#btnTypeOpen").addEventListener("click", () => setType("open"));

    $("#isMultiple").addEventListener("change", () => {
    
        if (!$("#isMultiple").checked) {
            let first = options.findIndex(o => o.IsCorrect);
            options = options.map((o, i) => ({ ...o, IsCorrect: first === -1 ? false : i === first }));
        }
        renderOptions();
    });

    $("#btnAddOption").addEventListener("click", addOption);
    $("#btnClearOptions").addEventListener("click", clearOptions);

    $("#btnPreview").addEventListener("click", () => showJson(buildDTO()));

    $("#btnSave").addEventListener("click", () => {
   
        const dto = buildDTO();
        showJson(dto);
        alert("DTO hazırdır. MVC-də bunu serverə göndərəcəksən.");
    });

    $("#btnBack").addEventListener("click", () => history.back());

 
    renderOptions();
});