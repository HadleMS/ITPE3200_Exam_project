document.getElementById("itemForm").addEventListener("submit", function(event) {
    const energiInput = document.getElementById("Energi_Kcal");
    const fettInput = document.getElementById("Fett");
    const proteinInput = document.getElementById("Protein");
    const karbohydratInput = document.getElementById("Karbohydrat");
    const saltInput = document.getElementById("Salt");

    if (energiInput.value && !energiInput.value.includes("kj")) {
        energiInput.value += " kj";
    }
    if (fettInput.value && !fettInput.value.includes("g")) {
        fettInput.value += " g";
    }
    if (proteinInput.value && !proteinInput.value.includes("g")) {
        proteinInput.value += " g";
    }
    if (karbohydratInput.value && !karbohydratInput.value.includes("g")) {
        karbohydratInput.value += " g";
    }
    if (saltInput.value && !saltInput.value.includes("g")) {
        saltInput.value += " g";
    }
});