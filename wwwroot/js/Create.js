document.getElementById("itemForm").addEventListener("submit", function(event) {
    const energiInput = document.getElementById("EnergiInput");
    const fettInput = document.getElementById("FettInput");
    const proteinInput = document.getElementById("ProteinInput");
    const karbohydratInput = document.getElementById("KarbohydratInput");
    const saltInput = document.getElementById("SaltInput");

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
