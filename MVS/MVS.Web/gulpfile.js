/// <binding ProjectOpened='default' />
const path = require('path');
var gulp = require('gulp');
var plugins = require('gulp-load-plugins')();
const mjml = require('./gulp-mjml');
const replace = require('gulp-replace');
var mergeStream = require('merge-stream');

function addLessTask(taskName, src) {
    gulp.task(taskName, function () {
        return gulp.src(src)
            .pipe(plugins.less())
            .pipe(gulp.dest('./wwwroot/css/'));
    });
}
addLessTask('less', './wwwroot/css/less/site.less')
gulp.task('watch', function () {
    gulp.watch('./wwwroot/css/less/**/*.less', gulp.series('less'));
    return
});

gulp.task('mjml-compile', function (usedculture) {
    var parentCwd = path.dirname(process.cwd()) + "\\";
    const cultureFolders = ["fr"];
    var tasks = [];
    cultureFolders.forEach(cultureFolder => {
        var basePath = `./MVS.EmailSender/Templates/mjml`;
        var outputPath = `./MVS.EmailSender/Templates/cshtml/`;
        tasks.push(gulp.src(`${basePath}/*.mjml`, { base: basePath, cwd: parentCwd, cwdbase: true })
            .pipe(mjml({ fileExt: ".cshtml" }))
            .pipe(replace(/@import url/gi, match => `@${match}`))
            .pipe(replace(/@media only/gi, match => `@${match}`))
            .pipe(gulp.dest(file => {
                var fileName = path.basename(file.path);
                file.path = parentCwd + outputPath + fileName;
                return '.';
            }, { cwd: parentCwd }))
        );
    });
    return mergeStream(tasks)
});

gulp.task('mjml-watch', function () {
    gulp.watch('../CNOA.EmailSender/Templates/mjml/*/*.mjml', gulp.series('mjml-compile'));
})


gulp.task('default', gulp.series('less', 'watch'));

