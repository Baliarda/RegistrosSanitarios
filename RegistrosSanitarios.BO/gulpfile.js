var gulp = require('gulp');
var merge = require('merge-stream');
var pug = require('gulp-pug');

// Dependency Dirs
var deps = {
    "jquery": {
        "dist/*": ""
    },
    "jquery.easing": {
        "*.js": ""
    },
    "bootstrap": {
        "dist/**/*": ""
    },
    "bootstrap-multiselect": {
        "dist/**/*": ""
    },
    "datatables.net": {
        "js/**/*": ""
    },
    "datatables.net-bs4": {
        "js/**/*": "js",
        "css/**/*": "css"
    },	
    "font-awesome": {
        "css/**/*": "css",
        "fonts/**/*": "fonts"
    },
    "ckeditor": {
        "**/*": ""
    },
    "globalize": {
        "dist/globalize.js": ""
    },
    "moment":{
        "moment.js": ""
    },
    "jquery-validation-unobtrusive": {
        "dist/**/*": ""
    },
    "jquery-validation": {
        "dist/**/*": ""
    },

    "corejs-typeahead": {
        "dist/**/*": ""
    },
    "popper.js": {
        "dist/umd/**/*": ""
    },
    "bootstrap-confirmation2": {
        "dist/**/*": ""
    }
    
};

gulp.task('pug', function buildHTML() {
    return gulp.src('./pug/*.pug')
        .pipe(pug())
        .pipe(gulp.dest('./'))
});

gulp.task("scripts", function () {

    var streams = [];

    for (var prop in deps) {
        console.log("Prepping Scripts for: " + prop);
        for (var itemProp in deps[prop]) {
            streams.push(gulp.src("node_modules/" + prop + "/" + itemProp)
                .pipe(gulp.dest("wwwroot/lib/" + prop + "/" + deps[prop][itemProp])));
        }
    }

    return merge(streams);

});

gulp.task("default", ['scripts']);