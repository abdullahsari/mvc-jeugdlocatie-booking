$(function() {
    $('.promote').on('click', function(e) {
        if (!confirm('Ben je zeker dat je deze gebruiker wilt promoveren tot een administrator?')) {
            e.preventDefault();
        }
    });
});