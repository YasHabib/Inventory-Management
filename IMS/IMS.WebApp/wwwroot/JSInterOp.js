function PreventFormSubmission(formId) {
    document.getElementById(formId).addEventListener("keydown", (event) =>
    {
        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    })
}