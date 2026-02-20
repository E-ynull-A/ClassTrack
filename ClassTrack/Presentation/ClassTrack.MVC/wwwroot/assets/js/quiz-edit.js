






const $ = (s) => document.querySelector(s);



function renderQuestions(filterText = "") {
    const list = $("#questionList");
    list.innerHTML = "";

    const needle = filterText.trim().toLowerCase();

    const filtered = questions
        .slice()
        .sort((a, b) => a.order - b.order)
        .filter(q => !needle || q.text.toLowerCase().includes(needle));

    if (filtered.length === 0) {
        list.innerHTML = `<div class="muted">No questions found.</div>`;
        return;
    }

    filtered.forEach((q) => {
        const item = document.createElement("div");
        item.className = "qitem";
        item.dataset.id = q.id;

        item.innerHTML = `
      <div class="qnum">${q.order}</div>

      <div class="qcontent">
        <div class="qtitle">${escapeHtml(q.text)}</div>
      </div>

      <div class="qactions">
        <button class="btn ghost" data-action="edit">Edit</button>
      </div>
    `;

        item
            .querySelector('[data-action="edit"]')
            .addEventListener("click", () => openModal(q.id));

        list.appendChild(item);
    });
}


function showJson(dto) {
    const box = $("#jsonBox");
    box.style.display = "block";
    box.textContent = JSON.stringify(dto, null, 2);
}


let currentQId = null;

function openModal(qid) {
    const q = questions.find(x => x.id === qid);
    if (!q) return;

    currentQId = qid;

    $("#modalMeta").textContent = `Question Id: ${q.id}`;
    $("#mqText").value = q.text;
    $("#mqPoints").value = q.points;
    $("#mqOrder").value = q.order;

    $("#modal").classList.add("show");
    $("#modal").setAttribute("aria-hidden", "false");
}

function closeModal() {
    currentQId = null;
    $("#modal").classList.remove("show");
    $("#modal").setAttribute("aria-hidden", "true");
}

function saveQuestionFromModal() {
    if (currentQId == null) return;

    const q = questions.find(x => x.id === currentQId);
    if (!q) return;

    q.text = $("#mqText").value.trim();
    q.points = Number($("#mqPoints").value || 0);
    q.order = Number($("#mqOrder").value || q.order);


    normalizeOrders();

    renderQuestions($("#qSearch").value);
    closeModal();


}

function deleteCurrentQuestion() {
    if (currentQId == null) return;

    questions = questions.filter(x => x.id !== currentQId);
    normalizeOrders();

    renderQuestions($("#qSearch").value);
    closeModal();
}

function normalizeOrders() {

    questions = questions
        .slice()
        .sort((a, b) => a.order - b.order)
        .map((q, i) => ({ ...q, order: i + 1 }));
}

function addQuestion() {
    const nextId = Math.max(0, ...questions.map(q => q.id)) + 1;
    const nextOrder = questions.length + 1;

    const newQ = {
        id: nextId,
        order: nextOrder,
        text: "New question...",
        points: 1
    };

    questions.push(newQ);
    renderQuestions($("#qSearch").value);
    openModal(newQ.id);
}


document.addEventListener("DOMContentLoaded", () => {
    renderQuizForm();
    renderQuestions();

    $("#btnPreview").addEventListener("click", () => {
        const dto = buildPutQuizDtoFromForm();
        showJson(dto);
    });

    $("#btnSaveQuiz").addEventListener("click", () => {
        const dto = buildPutQuizDtoFromForm();
  
        console.log("PutQuizDTO =>", dto);
        showJson(dto);
        alert("Console-da PutQuizDTO log olundu (MVC-də buranı fetch ilə bağlayarsan).");
    });

    $("#qSearch").addEventListener("input", (e) => {
        renderQuestions(e.target.value);
    });

    $("#btnAddQuestion").addEventListener("click", addQuestion);


    $("#btnCloseModal").addEventListener("click", closeModal);
    $("#btnCancelQ").addEventListener("click", closeModal);
    $("#btnSaveQ").addEventListener("click", saveQuestionFromModal);
    $("#btnDeleteQ").addEventListener("click", deleteCurrentQuestion);

    $("#modal").addEventListener("click", (e) => {
        if (e.target && e.target.dataset && e.target.dataset.close === "true") closeModal();
    });


    $("#btnBack").addEventListener("click", () => history.back());
});