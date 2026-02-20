(function(){
  window.$ = (sel, root=document) => root.querySelector(sel);
  window.$$ = (sel, root=document) => Array.from(root.querySelectorAll(sel));

  window.toast = (title, desc) => {
    const el = $("#toast");
    if(!el) return;
    $(".t", el).textContent = title || "Info";
    $(".d", el).textContent = desc || "";
    el.classList.add("show");
    clearTimeout(window.__toastT);
    window.__toastT = setTimeout(()=>el.classList.remove("show"), 2400);
  };

  window.copyText = async (text) => {
    try{
      await navigator.clipboard.writeText(text);
      toast("Copied", text);
    }catch{
      toast("Copy failed", "Browser blocked clipboard.");
    }
  };
})();
// ===== Header mobile menu =====
(() => {
  const btn = document.querySelector("[data-mobile-toggle]");
  const panel = document.querySelector("[data-mobile-panel]");
  if(!btn || !panel) return;

  btn.addEventListener("click", () => {
    panel.classList.toggle("show");
  });

  // close panel when clicking a link
  panel.addEventListener("click", (e) => {
    const a = e.target.closest("a");
    if(a) panel.classList.remove("show");
  });
})();
