


let quiz = {
    Name: "Sample Quiz",
    StartTime: "2026-02-07T10:30",  
    Duration: 30,
    ClassRoomId: 12
};

let questions = [
    { id: 101, order: 1, text: "What is TCP Fast Retransmit?", points: 2 },
    { id: 102, order: 2, text: "Explain VLAN broadcast domains.", points: 3 },
    { id: 103, order: 3, text: "What is set-associative cache?", points: 2 }
];


const $ = (s) => document.querySelector(s);

function renderQuizForm() {
    $("#quizName").value = quiz.Name ?? "";
    $("#startTime").value = quiz.StartTime ?? "";
    $("#duration").value = quiz.Duration ?? "";
    $("#classRoomId").value = quiz.ClassRoomId ?? "";
}

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

function escapeHtml(str) {
    return String(str)
        .replaceAll("&", "&amp;")
        .replaceAll("<", "&lt;")
        .replaceAll(">", "&gt;")
        .replaceAll('"', "&quot;")
        .replaceAll("'", "&#039;");
}


function buildPutQuizDtoFromForm() {
    return {
        Name: $("#quizName").value.trim(),
        StartTime: $("#startTime").value,          
        Duration: Number($("#duration").value || 0),
        ClassRoomId: Number($("#classRoomId").value || 0),
    };
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