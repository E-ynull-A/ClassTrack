(() => {
  const deadlineInput = document.querySelector('input[name="Deadline"]');
  const deadlineText = document.getElementById("deadlineText");
  const closedBadge = document.getElementById("closedBadge");

  const q = document.getElementById("q");
  const filter = document.getElementById("filter");
  const list = document.getElementById("list");

  const reviewPanel = document.getElementById("reviewPanel");
  const rvStudent = document.getElementById("rvStudent");
  const rvMessage = document.getElementById("rvMessage");
  const rvDownload = document.getElementById("rvDownload");
  const rvNoFile = document.getElementById("rvNoFile");

  function applyClosedUI(){
    if(!deadlineInput?.value) return;
    const d = new Date(deadlineInput.value);
    const now = new Date();
    const closed = now.getTime() > d.getTime();
    closedBadge.style.display = closed ? "inline-flex" : "none";
  }

  deadlineInput?.addEventListener("change", () => {
    if (deadlineText) deadlineText.textContent = deadlineInput.value.replace("T", " ");
    applyClosedUI();
  });

  // Top actions (demo)
  document.getElementById("btnSave")?.addEventListener("click", () => toast("Save", "MVC POST /Tasks/Save"));
  document.getElementById("btnPublish")?.addEventListener("click", () => toast("Publish", "MVC POST /Tasks/Publish (optional)"));
  document.getElementById("btnDelete")?.addEventListener("click", () => toast("Delete", "MVC POST/DELETE /Tasks/Delete"));

  document.getElementById("btnUpload")?.addEventListener("click", () => toast("Upload", "MVC file upload bağla"));

  // Filtering
  function applyFilter(){
    const term = (q?.value || "").trim().toLowerCase();
    const f = filter?.value || "all";

    Array.from(list.querySelectorAll(".item")).forEach(it => {
      const name = (it.dataset.student || "").toLowerCase();
      const st = it.dataset.status || "all";
      const okName = !term || name.includes(term);
      const okStatus = (f === "all") || (f === st);
      it.style.display = (okName && okStatus) ? "" : "none";
    });
  }

  q?.addEventListener("input", applyFilter);
  filter?.addEventListener("change", applyFilter);

  // Open review panel
  list?.addEventListener("click", (e) => {
    const item = e.target.closest(".item");
    if(!item) return;

    const studentName = item.dataset.student || "—";
    rvStudent.textContent = studentName;

    // Demo data (MVC-də modeldən dolduracaqsan)
    if(item.dataset.status === "submitted"){
      rvMessage.textContent = "Here is my summary and notes...";
      rvNoFile.style.display = "none";
      rvDownload.style.display = "inline-flex";
    } else {
      rvMessage.textContent = "No submission yet.";
      rvNoFile.style.display = "inline";
      rvDownload.style.display = "none";
    }

    reviewPanel.style.display = "block";
    window.scrollTo({ top: reviewPanel.offsetTop - 10, behavior: "smooth" });
  });

  document.getElementById("btnCloseReview")?.addEventListener("click", () => {
    reviewPanel.style.display = "none";
  });

  document.getElementById("btnSaveReview")?.addEventListener("click", () => toast("Review", "MVC POST /Tasks/Review"));
  document.getElementById("btnMarkPending")?.addEventListener("click", () => toast("Pending", "Submission status -> Pending"));

  applyClosedUI();
})();
