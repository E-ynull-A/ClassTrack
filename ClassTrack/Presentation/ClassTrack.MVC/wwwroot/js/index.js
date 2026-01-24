// Helpers
const $ = (s, p=document) => p.querySelector(s);

(() => {
  // Year
  const y = $("#year");
  if (y) y.textContent = new Date().getFullYear();

  // Mobile menu
  const burger = $("#burgerBtn");
  const nav = $("#nav");

  if (burger && nav) {
    burger.addEventListener("click", () => {
      nav.classList.toggle("open");
    });

    // close on link click (mobile)
    nav.addEventListener("click", (e) => {
      const a = e.target.closest("a");
      if (a) nav.classList.remove("open");
    });

    // close when clicking outside
    document.addEventListener("click", (e) => {
      if (!nav.classList.contains("open")) return;
      const inside = nav.contains(e.target) || burger.contains(e.target);
      if (!inside) nav.classList.remove("open");
    });
  }

  // Theme toggle (dark/light)
  const themeBtn = $("#themeBtn");
  const root = document.documentElement;
  const saved = localStorage.getItem("ct_theme");
  if (saved === "light") root.setAttribute("data-theme", "light");

  const applyLabel = () => {
    const isLight = root.getAttribute("data-theme") === "light";
    if (themeBtn) themeBtn.textContent = isLight ? "Dark" : "Light";
  };
  applyLabel();

  themeBtn?.addEventListener("click", () => {
    const isLight = root.getAttribute("data-theme") === "light";
    if (isLight) root.removeAttribute("data-theme");
    else root.setAttribute("data-theme", "light");
    localStorage.setItem("ct_theme", isLight ? "dark" : "light");
    applyLabel();
  });
})();

// Home actions
(() => {
  const joinPanel = document.getElementById("joinPanel");
  const btnJoinToggle = document.getElementById("btnJoinToggle");
  const btnJoinGo = document.getElementById("btnJoinGo");
  const input = document.getElementById("classCodeInput");
  const btnCreateClass = document.getElementById("btnCreateClass");

  btnJoinToggle?.addEventListener("click", () => {
    joinPanel?.classList.toggle("show");
    if (joinPanel?.classList.contains("show")) input?.focus();
  });

  btnJoinGo?.addEventListener("click", () => {
    const code = (input?.value || "").trim();
    if (!code) {
      alert("Class code boş ola bilməz.");
      return;
    }
    alert("MVC-də POST /Classes/Join edəcəksən. Code: " + code);
  });

  btnCreateClass?.addEventListener("click", () => {
    alert("MVC-də POST /Classes/Create edəcəksən.");
  });
})();

