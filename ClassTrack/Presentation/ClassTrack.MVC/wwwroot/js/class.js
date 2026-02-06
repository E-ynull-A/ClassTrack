(function () {
  const btn = document.getElementById("btnAccount");
  const menu = document.getElementById("accountMenu");

  if (!btn || !menu) return;

  function closeMenu() {
    menu.classList.remove("show");
    menu.setAttribute("aria-hidden", "true");
  }

  function toggleMenu() {
    const isOpen = menu.classList.contains("show");
    if (isOpen) closeMenu();
    else {
      menu.classList.add("show");
      menu.setAttribute("aria-hidden", "false");
    }
  }

  btn.addEventListener("click", (e) => {
    e.stopPropagation();
    toggleMenu();
  });

  document.addEventListener("click", () => closeMenu());

  document.addEventListener("keydown", (e) => {
    if (e.key === "Escape") closeMenu();
  });

  menu.addEventListener("click", (e) => e.stopPropagation());
})();

