const grades = document.querySelectorAll('#grade')

grades.forEach(grade => {
    const gradeName = grade.innerHTML
    
    for (let i = 0; i <= 6; i++) {
        if (gradeName.includes(i.toString())) {
            grade.classList.add(`grade-${i}`)
            break
        }
    }
})