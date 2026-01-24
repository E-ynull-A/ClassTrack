(() => {
  $("#btnInvite")?.addEventListener("click", () => {
    toast("Invite", "MVC-də class code copy edəcəksən.");
  });

  // Demo actions
  $$(".btn.ok").forEach(b => b.addEventListener("click", ()=>toast("Promote","API/MVC: student -> teacher")));
  $$(".btn.danger").forEach(b => b.addEventListener("click", ()=>toast("Action","MVC POST ilə edəcəksən.")));
})();
