(() => {
  $("#btnCreate").addEventListener("click", () => {
    const name = $("#className").value.trim();
    if(!name) return toast("Xəta", "Class name boşdur.");
    toast("Create", "MVC-də /Classes/Create POST-ə bağla.");
  });

  $("#btnJoin").addEventListener("click", () => {
    const code = $("#classCode").value.trim();
    if(!code) return toast("Xəta", "Class code boşdur.");
    toast("Join", "MVC-də /Classes/Join POST-ə bağla.");
  });
})();
