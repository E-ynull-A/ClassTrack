document.addEventListener("DOMContentLoaded", () => {
    const btnCreate = document.getElementById("btnCreate");
    const btnJoin = document.getElementById("btnJoin");

    if (btnCreate) {
        btnCreate.addEventListener("click", () => {
            const name = document.getElementById("className").value.trim();
            if (!name) return toast("Xəta", "Class name boşdur.");
            toast("Success", "Yaradılır...");
        });
    }

    if (btnJoin) {
        btnJoin.addEventListener("click", () => {
            const code = document.getElementById("classCode").value.trim();
            if (!code) return toast("Xəta", "Class code boşdur.");
            toast("Success", "Qoşulur...");
        });
    }
});

(() => {
    const $ = (s) => document.querySelector(s);

    const btnCreate = $("#btnCreate");
    const className = $("#className");

    const createPanel = $("#createPanel");
    const createPanelText = $("#createPanelText");
    const createdCode = $("#createdCode");
    const btnCopyCreated = $("#btnCopyCreated");
    const btnGoClass = $("#btnGoClass");

    btnCreate?.addEventListener("click", () => {
        const name = (className?.value || "").trim();

        if (!name) {
            toast("Error", "Class name boş ola bilməz.");
            return;
        }

       
        if (createPanel) createPanel.style.display = "block";
        if (createPanelText) createPanelText.textContent = `Class "${name}" hazırdır. MVC-də POST /Classes/Create edəcəksən.`;

       
        const demoCode = "A7K2Q9";
        if (createdCode) {
            createdCode.style.display = "inline-flex";
            createdCode.textContent = "CODE: " + demoCode;
        }
        if (btnCopyCreated) btnCopyCreated.style.display = "inline-flex";
        if (btnGoClass) {
            btnGoClass.style.display = "inline-flex";
            btnGoClass.href = "#"; 
        }
    });

    btnCopyCreated?.addEventListener("click", () => {
        const text = (createdCode?.textContent || "").replace("CODE:", "").trim();
        if (!text) return;

        if (window.copyText) window.copyText(text);
        else navigator.clipboard?.writeText(text);

        toast("Copied", "Class code kopyalandı.");
    });
})();

document.addEventListener("DOMContentLoaded", function () {
    
    const slideCount = document.querySelectorAll(".mySwiper .swiper-slide").length;

    const swiperOptions = {
        spaceBetween: 20,
        navigation: {
            nextEl: ".swiper-button-next",
            prevEl: ".swiper-button-prev",
        },
        watchOverflow: true,
       
        breakpoints: {
            320: { slidesPerView: 1 },
            768: { slidesPerView: slideCount < 3 ? slideCount : 2 },
            1024: { slidesPerView: slideCount < 3 ? slideCount : 3 }
        }
    };

    const swiper = new Swiper(".mySwiper", swiperOptions);
});

document.addEventListener("click", (e) => {
    const btn = e.target.closest(".card-delete");
    if (!btn) return;

  
    e.preventDefault();
    e.stopPropagation();

    const id = btn.dataset.id;
    if (!id) return;

    const ok = confirm("Bu sinifi silmək istəyirsən?");
    if (!ok) return;
 
    btn.closest(".class-card")?.remove();

});




