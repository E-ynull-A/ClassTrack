(() => {
    const $ = (s) => document.querySelector(s);

    // Steps
    const step1 = $("#step1"), step2 = $("#step2"), step3 = $("#step3");
    const tab1 = document.querySelector('[data-step="1"]');
    const tab2 = document.querySelector('[data-step="2"]');
    const tab3 = document.querySelector('[data-step="3"]');

    const showStep = (n) => {
        [step1, step2, step3].forEach(x => x?.classList.remove("show"));
        [tab1, tab2, tab3].forEach(x => x?.classList.remove("active"));

        if (n === 1) { step1?.classList.add("show"); tab1?.classList.add("active"); }
        if (n === 2) { step2?.classList.add("show"); tab2?.classList.add("active"); }
        if (n === 3) { step3?.classList.add("show"); tab3?.classList.add("active"); }
    };

    tab1?.addEventListener("click", () => showStep(1));
    tab2?.addEventListener("click", () => !tab2.disabled && showStep(2));
    tab3?.addEventListener("click", () => !tab3.disabled && showStep(3));

    // State (PostQuizDTO shape)
    let quiz = null;
    let choiceQuestions = [];
    let openQuestions = [];

    // Step1 elements
    const quizName = $("#quizName");
    const classRoomId = $("#classRoomId");
    const startTime = $("#startTime");
    const durationMin = $("#durationMin");
    const btnSaveQuiz = $("#btnSaveQuiz");
    const quizInfoHint = $("#quizInfoHint");

    btnSaveQuiz?.addEventListener("click", () => {
        const name = (quizName?.value || "").trim();
        const cid = parseInt(classRoomId?.value || "0", 10);
        const st = startTime?.value;
        const dur = parseInt(durationMin?.value || "0", 10);

        if (!name || !cid || !st || !dur) {
            toast("Missing", "Name, ClassRoomId, StartTime, Duration doldur.");
            return;
        }

       
        quiz = {
            Name: name,
            StartTime: new Date(st).toISOString(),
            Duration: `00:${String(dur).padStart(2, "0")}:00`,
            ClassRoomId: cid
        };

        tab2.disabled = false;
        tab3.disabled = false;
        if (quizInfoHint) quizInfoHint.textContent = "Saved ✅ İndi sual əlavə edə bilərsən.";
        toast("Saved", "Quiz info saxlanıldı.");
        showStep(2);
        renderList();
    });


    const btnTypeChoice = $("#btnTypeChoice");
    const btnTypeOpen = $("#btnTypeOpen");
    const choiceBox = $("#choiceBox");
    const openBox = $("#openBox");

    const setType = (t) => {
        if (!choiceBox || !openBox) return;

        if (t === "choice") {
            choiceBox.style.display = "";
            openBox.style.display = "none";
            btnTypeChoice?.classList.add("primary");
            btnTypeOpen?.classList.remove("primary");
        } else {
            choiceBox.style.display = "none";
            openBox.style.display = "";
            btnTypeOpen?.classList.add("primary");
            btnTypeChoice?.classList.remove("primary");
        }
    };

    btnTypeChoice?.addEventListener("click", () => setType("choice"));
    btnTypeOpen?.addEventListener("click", () => setType("open"));

  
    const cTitle = $("#cTitle");
    const cPoint = $("#cPoint");
    const cIsMultiple = $("#cIsMultiple");
    const optionsList = $("#optionsList");
    const btnAddOption = $("#btnAddOption");
    const btnClearOptions = $("#btnClearOptions");
    const btnSaveChoice = $("#btnSaveChoice");

    let options = [];

    const renderOptions = () => {
        if (!optionsList) return;
        optionsList.innerHTML = "";

        options.forEach((opt, idx) => {
            const div = document.createElement("div");
            div.className = "opt";
            div.innerHTML = `
        <input class="input" placeholder="Variant (məs: A)" value="${escapeHtml(opt.Variant)}" data-k="v" />
        <div class="mini">
          <label class="muted" style="display:flex; gap:8px; align-items:center;">
            <input type="checkbox" ${opt.IsCorrect ? "checked" : ""} data-k="c" />
            IsCorrect
          </label>
        </div>
        <div class="mini">
          <button class="btn danger" type="button" data-del="${idx}">Remove</button>
        </div>
      `;

            div.querySelector('[data-k="v"]')?.addEventListener("input", (e) => {
                options[idx].Variant = e.target.value;
            });
            div.querySelector('[data-k="c"]')?.addEventListener("change", (e) => {
                options[idx].IsCorrect = e.target.checked;
            });
            div.querySelector('[data-del]')?.addEventListener("click", () => {
                options.splice(idx, 1);
                renderOptions();
            });

            optionsList.appendChild(div);
        });
    };

    btnAddOption?.addEventListener("click", () => {
        options.push({ Variant: "", IsCorrect: false });
        renderOptions();
    });

    btnClearOptions?.addEventListener("click", () => {
        options = [];
        renderOptions();
    });

    btnSaveChoice?.addEventListener("click", () => {
        if (!quiz) { toast("First", "Əvvəl quiz info saxla."); return; }

        const title = (cTitle?.value || "").trim();
        const point = parseFloat(cPoint?.value || "0");
        const isMultiple = (cIsMultiple?.value === "true");

        if (!title) { toast("Missing", "Choice title boş ola bilməz."); return; }
        if (options.length < 2) { toast("Missing", "Ən az 2 option olmalıdır."); return; }

        const correctCount = options.filter(x => !!x.IsCorrect).length;
        if (correctCount === 0) { toast("Missing", "Ən az 1 correct option seç."); return; }
        if (!isMultiple && correctCount > 1) { toast("Rule", "IsMultiple=false isə 1 correct olmalıdır."); return; }

        const q = {
            Title: title,
            Point: point,
            IsMultiple: isMultiple,
            Options: options.map(x => ({ Variant: (x.Variant || "").trim(), IsCorrect: !!x.IsCorrect }))
        };

        if (q.Options.some(x => !x.Variant)) { toast("Missing", "Variant boş ola bilməz."); return; }

        choiceQuestions.push(q);

  
        if (cTitle) cTitle.value = "";
        if (cPoint) cPoint.value = "1";
        if (cIsMultiple) cIsMultiple.value = "false";
        options = [];
        renderOptions();

        toast("Added", "Choice question əlavə olundu.");
        renderList();
    });

  
    const oTitle = $("#oTitle");
    const oPoint = $("#oPoint");
    const btnSaveOpen = $("#btnSaveOpen");

    btnSaveOpen?.addEventListener("click", () => {
        if (!quiz) { toast("First", "Əvvəl quiz info saxla."); return; }

        const title = (oTitle?.value || "").trim();
        const point = parseFloat(oPoint?.value || "0");
        if (!title) { toast("Missing", "Open title boş ola bilməz."); return; }

        openQuestions.push({ Title: title, Point: point });

        if (oTitle) oTitle.value = "";
        if (oPoint) oPoint.value = "1";

        toast("Added", "Open question əlavə olundu.");
        renderList();
    });

    const addedList = $("#addedList");

    function renderList() {
        const total = choiceQuestions.length + openQuestions.length;
        if (!addedList) return;

        addedList.innerHTML = "";

        if (total === 0) {
            addedList.innerHTML = `<div class="muted">Hələ sual yoxdur.</div>`;
            return;
        }

        const makeItem = (type, title, meta, onDel) => {
            const div = document.createElement("div");
            div.className = "q-item";
            div.innerHTML = `
        <div>
          <div class="title">${escapeHtml(title)} <span class="badge">${type}</span></div>
          <div class="meta">${escapeHtml(meta)}</div>
        </div>
        <div class="nav">
          <button class="btn danger" type="button">Delete</button>
        </div>
      `;
            div.querySelector("button")?.addEventListener("click", () => onDel());
            return div;
        };

        choiceQuestions.forEach((q, i) => {
            const meta = `${q.Point} pt • options: ${q.Options?.length || 0} • multiple: ${q.IsMultiple}`;
            addedList.appendChild(makeItem("Choice", q.Title, meta, () => {
                choiceQuestions.splice(i, 1);
                renderList();
            }));
        });

        openQuestions.forEach((q, i) => {
            const meta = `${q.Point} pt`;
            addedList.appendChild(makeItem("Open", q.Title, meta, () => {
                openQuestions.splice(i, 1);
                renderList();
            }));
        });
    }


    const btnToggleJson = $("#btnToggleJson");
    btnToggleJson?.addEventListener("click", () => {
        const box = $("#jsonWrap");
        if (!box) return;
        const open = box.style.display !== "none";
        box.style.display = open ? "none" : "block";
        btnToggleJson.textContent = open ? "Show JSON" : "Hide JSON";
    });

    const buildPayload = () => ({
        ...quiz,
        ChoiceQuestions: choiceQuestions,
        OpenQuestions: openQuestions
    });

    function renderReview() {
        if (!quiz) return;
        const payload = buildPayload();

        $("#rvName") && ($("#rvName").textContent = payload.Name || "-");
        $("#rvClass") && ($("#rvClass").textContent = String(payload.ClassRoomId ?? "-"));
        $("#rvStart") && ($("#rvStart").textContent = payload.StartTime ? new Date(payload.StartTime).toLocaleString() : "-");
        $("#rvDur") && ($("#rvDur").textContent = payload.Duration || "-");

        const choiceCount = payload.ChoiceQuestions.length;
        const openCount = payload.OpenQuestions.length;
        $("#rvChoiceCount") && ($("#rvChoiceCount").textContent = String(choiceCount));
        $("#rvOpenCount") && ($("#rvOpenCount").textContent = String(openCount));

        const totalPts =
            payload.ChoiceQuestions.reduce((a, x) => a + (Number(x.Point) || 0), 0) +
            payload.OpenQuestions.reduce((a, x) => a + (Number(x.Point) || 0), 0);

        $("#rvTotalPts") && ($("#rvTotalPts").textContent = String(totalPts));

        const wrap = $("#rvQuestions");
        if (wrap) {
            wrap.innerHTML = "";

            payload.ChoiceQuestions.forEach((q, i) => {
                const correct = (q.Options || []).filter(o => o.IsCorrect).length;
                const div = document.createElement("div");
                div.className = "rv-item";
                div.innerHTML = `
          <div class="t">
            <span>#${i + 1} ${escapeHtml(q.Title)}</span>
            <span class="badge">Choice • ${q.Point}pt</span>
          </div>
          <div class="m">Options: ${(q.Options || []).length} • Correct: ${correct} • Multiple: ${q.IsMultiple}</div>
        `;
                wrap.appendChild(div);
            });

            payload.OpenQuestions.forEach((q, i) => {
                const div = document.createElement("div");
                div.className = "rv-item";
                div.innerHTML = `
          <div class="t">
            <span>#${choiceCount + i + 1} ${escapeHtml(q.Title)}</span>
            <span class="badge">Open • ${q.Point}pt</span>
          </div>
          <div class="m">Open answer (options yoxdur)</div>
        `;
                wrap.appendChild(div);
            });
        }

        const jsonOut = $("#jsonOut");
        if (jsonOut) jsonOut.textContent = JSON.stringify(payload, null, 2);
    }

    function syncHiddenFields() {
        const host = document.getElementById("bindFields");
        if (!host) { console.log("bindFields tapılmadı"); return; }

        host.innerHTML = "";

        const add = (name, value) => {
            const inp = document.createElement("input");
            inp.type = "hidden";
            inp.name = name;
            inp.value = value ?? "";
            host.appendChild(inp);
        };

       
        add("Name", quiz?.Name);
        add("ClassRoomId", quiz?.ClassRoomId);
        add("StartTime", quiz?.StartTime);
        add("Duration", quiz?.Duration);

    
        choiceQuestions.forEach((q, i) => {
            add(`ChoiceQuestions[${i}].Title`, q.Title);
            add(`ChoiceQuestions[${i}].Point`, q.Point);
            add(`ChoiceQuestions[${i}].IsMultiple`, q.IsMultiple);

            (q.Options || []).forEach((o, j) => {
                add(`ChoiceQuestions[${i}].Options[${j}].Variant`, o.Variant);
                add(`ChoiceQuestions[${i}].Options[${j}].IsCorrect`, o.IsCorrect);
            });
        });

    
        openQuestions.forEach((q, i) => {
            add(`OpenQuestions[${i}].Title`, q.Title);
            add(`OpenQuestions[${i}].Point`, q.Point);
        });

    
        const form = document.getElementById("quizForm");
        if (form) {
            const fd = new FormData(form);
            console.log("Posted keys:", [...fd.keys()].filter(k =>
                k.startsWith("ChoiceQuestions") || k.startsWith("OpenQuestions")));
        }
    }

    
    $("#btnToReview")?.addEventListener("click", () => {
        if (!quiz) { toast("First", "Əvvəl quiz info saxla."); return; }
        showStep(3);
        renderReview();
    });

    $("#btnBackToQuestions")?.addEventListener("click", () => showStep(2));

 
    $("#btnSubmit")?.addEventListener("click", () => {
        if (!quiz) {
            toast("First", "Əvvəl quiz info saxla.");
            return;
        }

        if (choiceQuestions.length + openQuestions.length === 0) {
            toast("Missing", "Ən az 1 sual əlavə elə.");
            return;
        }

        syncHiddenFields();
        const form = document.getElementById("quizForm");
        if (!form) return;

        syncHiddenFields();
        form.requestSubmit();
    });

  
    $("#btnReset")?.addEventListener("click", () => {
        quiz = null;
        choiceQuestions = [];
        openQuestions = [];
        options = [];
        renderOptions();
        renderList();

        if (quizName) quizName.value = "";
        if (startTime) startTime.value = "";
        if (durationMin) durationMin.value = "0";

        tab2.disabled = true;
        tab3.disabled = true;

        if (quizInfoHint) quizInfoHint.textContent = "Əvvəl quiz məlumatlarını saxla.";

        $("#rvName") && ($("#rvName").textContent = "-");
        $("#rvClass") && ($("#rvClass").textContent = "-");
        $("#rvStart") && ($("#rvStart").textContent = "-");
        $("#rvDur") && ($("#rvDur").textContent = "-");
        $("#rvChoiceCount") && ($("#rvChoiceCount").textContent = "0");
        $("#rvOpenCount") && ($("#rvOpenCount").textContent = "0");
        $("#rvTotalPts") && ($("#rvTotalPts").textContent = "0");
        $("#rvQuestions") && ($("#rvQuestions").innerHTML = "");
        $("#jsonOut") && ($("#jsonOut").textContent = "{ }");

        const jsonWrap = $("#jsonWrap");
        if (jsonWrap) jsonWrap.style.display = "none";
        if (btnToggleJson) btnToggleJson.textContent = "Show JSON";

        toast("Reset", "Wizard təmizləndi.");
        showStep(1);
    });


    function escapeHtml(s) {
        return (s || "").replace(/[&<>"']/g, m => ({
            "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;", "'": "&#039;"
        }[m]));
    }

    showStep(1);
    renderList();
})();