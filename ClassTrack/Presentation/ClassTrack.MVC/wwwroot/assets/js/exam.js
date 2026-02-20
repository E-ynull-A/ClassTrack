
const $ = (s, p = document) => p.querySelector(s);


function parseTimeSpanToSeconds(ts) {

    const parts = String(ts).split(":").map(Number);
    if (parts.length !== 3 || parts.some(n => Number.isNaN(n))) return 0;
    const [h, m, s] = parts;
    return (h * 3600) + (m * 60) + s;
}

function fmtSeconds(sec) {
    sec = Math.max(0, Math.floor(sec));
    const h = Math.floor(sec / 3600);
    const m = Math.floor((sec % 3600) / 60);
    const s = sec % 60;
    const mm = String(m).padStart(2, "0");
    const ss = String(s).padStart(2, "0");
    if (h > 0) return `${String(h).padStart(2, "0")}:${mm}:${ss}`;
    return `${mm}:${ss}`;
}

// =========================
// 2) Answered count (DOM-dan oxuyur)
// =========================
// Qaydalar:
// - Choice blokda (radio/checkbox) ən az 1 seçim varsa => answered
// - Open blokda textarea boş deyilsə => answered
//
// Bunun işləməsi üçün hər sual blokuna bu kimi marker qoy:
//  Choice q div: data-kind="choice"
//  Open   q div: data-kind="open"
//
// Misal:
// <div class="q" data-kind="choice"> ... </div>
// <div class="q" data-kind="open"> ... </div>
function updateAnsweredCount() {
    const answeredEl = $("#answeredCount");
    if (!answeredEl) return;

    let answered = 0;
    const blocks = document.querySelectorAll(".q[data-kind]");

    blocks.forEach(b => {
        const kind = b.dataset.kind;

        if (kind === "choice") {
            const anyChecked = b.querySelector('input[type="radio"]:checked, input[type="checkbox"]:checked');
            if (anyChecked) answered++;
            return;
        }

        if (kind === "open") {
            const ta = b.querySelector("textarea");
            if (ta && ta.value.trim().length > 0) answered++;
            return;
        }
    });

    answeredEl.textContent = String(answered);
}

function wireAnsweredListeners() {
    const root = $("#questionsWrap") || document;
    root.addEventListener("change", (e) => {
        const t = e.target;
        if (!t) return;
        if (t.matches('input[type="radio"], input[type="checkbox"]')) updateAnsweredCount();
    });

    root.addEventListener("input", (e) => {
        const t = e.target;
        if (!t) return;
        if (t.matches("textarea")) updateAnsweredCount();
    });
}


function getDurationSeconds() {
    const secEl = $("#durationSec");
    if (secEl && secEl.value && !Number.isNaN(Number(secEl.value))) return Number(secEl.value);

    const tsEl = $("#durationTs");
    if (tsEl && tsEl.value) return parseTimeSpanToSeconds(tsEl.value);

  
    const durInfo = $("#durationInfo");
    if (durInfo) {
      
        const m = String(durInfo.textContent || "").match(/(\d{2}:\d{2}:\d{2})/);
        if (m) return parseTimeSpanToSeconds(m[1]);
    }

    return 0;
}

let timerInt = null;

function startTimer() {
    const totalSec = getDurationSeconds();
    let left = totalSec;

    const tEl = $("#timerText");
    const bar = $("#timerBar");
    const form = $("#examForm");
    const note = $("#submitNote");

    if (!tEl || !bar) return;

    function tick() {
        tEl.textContent = fmtSeconds(left);

        const pct = totalSec === 0 ? 0 : (1 - (left / totalSec)) * 100;
        bar.style.width = `${Math.max(0, Math.min(100, pct))}%`;

        if (left <= 60) tEl.classList.add("danger");
        else tEl.classList.remove("danger");

        if (left <= 0) {
            clearInterval(timerInt);
            timerInt = null;

            if (note) note.innerHTML = "Vaxt bitdi ✅ avtomatik göndərilir...";
            if (form) form.submit();

            return;
        }

        left--;
    }

    if (timerInt) clearInterval(timerInt);
    tick();
    timerInt = setInterval(tick, 1000);
}


function wireReset() {
    const btnReset = $("#btnReset");
    if (!btnReset) return;

    btnReset.addEventListener("click", () => {
       
        setTimeout(() => {
            const note = $("#submitNote");
            if (note) note.innerHTML = "Reset olundu.";
            updateAnsweredCount();
        }, 0);
    });
}


document.addEventListener("DOMContentLoaded", () => {
    wireAnsweredListeners();
    wireReset();
    updateAnsweredCount();
    startTimer();
});