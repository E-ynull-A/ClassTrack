(() => {
    // Helpers (base.js-də varsa bunlar artıq var; yoxdursa fallback)
    const $ = window.$ || ((s) => document.querySelector(s));
    const $$ = window.$$ || ((s) => Array.from(document.querySelectorAll(s)));

    // Role (Teacher/Student) - safe read
    const roleEl = $("#roleBadge");
    const role = (roleEl?.textContent || "").trim().toLowerCase(); // "teacher" / "student"

    // -------------------------
    // Teacher-only header actions
    // -------------------------
    const btnCopy = $("#btnCopyCode");
    const btnCreateQuiz = $("#btnCreateQuiz");
    const btnCreateTask = $("#btnCreateTask");
    const classCodeEl = $("#classCode");

    if (role !== "teacher") {
        if (btnCopy) btnCopy.style.display = "none";
        if (btnCreateQuiz) btnCreateQuiz.style.display = "none";
        if (btnCreateTask) btnCreateTask.style.display = "none";
        if (classCodeEl) classCodeEl.textContent = "•••••• (hidden)";
    }

    // -------------------------
    // Tabs teacher-only: members + attendance
    // -------------------------
    if (role !== "teacher") {
        const tabs = $$(".tab");
        const membersTab = tabs.find(x => x.dataset.tab === "members");
        const attendanceTab = tabs.find(x => x.dataset.tab === "attendance");
        if (membersTab) membersTab.style.display = "none";
        if (attendanceTab) attendanceTab.style.display = "none";
    }

    // -------------------------
    // Tabs switch (guard: element yoxdursa error atmasın)
    // -------------------------
    $$(".tab").forEach(btn => {
        btn.addEventListener("click", () => {
            $$(".tab").forEach(x => x.classList.remove("active"));
            btn.classList.add("active");

            const t = btn.dataset.tab;
            $$(".tabpane").forEach(p => p.classList.remove("show"));

            const pane = $("#tab-" + t);
            if (pane) pane.classList.add("show");
        });
    });

    // -------------------------
    // Copy Code
    // -------------------------
    if (btnCopy) {
        btnCopy.addEventListener("click", () => {
            const code = (classCodeEl?.textContent || "").trim();
            if (window.copyText) window.copyText(code);
            else navigator.clipboard?.writeText(code);
        });
    }

    // -------------------------
    // Teacher: class switcher (multiple classes)
    // -------------------------
    const classSwitchBox = $("#classSwitchBox");
    const classSelect = $("#classSelect");

    // göstər/gizlə
    if (classSwitchBox) classSwitchBox.style.display = (role === "teacher") ? "block" : "none";

    // Switcher işlətmirsənsə, burdan sonra çıx
    if (!classSelect) return;

    // Demo data (MVC-də sonra siləcəksən)
    const classes = {
        "1": { title: "Network 101", code: "A7K2Q9", members: 24 },
        "2": { title: "Operating Systems", code: "K9P4T2", members: 31 },
        "3": { title: "Database Systems", code: "D3B8S1", members: 18 }
    };

    const classTitleEl = $("#classTitle");
    const memberCountEl = $("#memberCount");

    function applySelected(id) {
        const c = classes[id];
        if (!c) return;

        if (classTitleEl) classTitleEl.textContent = c.title;
        if (classCodeEl) classCodeEl.textContent = c.code;
        if (memberCountEl) memberCountEl.textContent = c.members;

        if (window.toast) window.toast("Class changed", c.title);
    }

    classSelect.addEventListener("change", (e) => {
        applySelected(e.target.value);

        // MVC-yə bağlayanda bunu aç:
        // window.location.href = "/Class/Details/" + e.target.value;
    });

    // initial
    applySelected(classSelect.value);
})();
