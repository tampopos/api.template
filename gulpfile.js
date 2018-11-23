const gulp = require('gulp');
const {
  clean,
  restore,
  build,
  test,
  pack,
  publish,
  run
} = require('gulp-dotnet-cli');
const runSequence = require('run-sequence');
const path = require('path');
const zip = require('gulp-zip');
const spawn = require('child_process').spawn;

const webName = 'Api';
const web = '**/' + webName + '.csproj';
const appEntries = [web];
const testEntries = [
  '**/Tmpps.Infrastructure.TestTools.csproj',
  '**/*.Tests.csproj'
];
const entries = appEntries.concat(testEntries);
const dist = 'dist';

//clean
gulp.task('clean', () => {
  return gulp.src(entries, { read: false }).pipe(clean());
});
//restore nuget packages
gulp.task('restore', () => {
  return gulp.src(entries, { read: false }).pipe(restore());
});
//compile
gulp.task('build', () => {
  return gulp.src(entries, { read: false }).pipe(build());
});
//run unit tests
gulp.task('test', () => {
  return gulp.src(testEntries, { read: false }).pipe(test());
});
//compile and publish an application to the local filesystem
gulp.task('publish', cb => {
  return runSequence('test', ['publish-web-fdd'], 'zip', cb);
});
gulp.task('publish-web-fdd', cb => {
  const output = path.join(process.cwd(), dist, 'raw', 'web', 'fdd');
  return gulp
    .src(web, { read: false })
    .pipe(publish({ configuration: 'Release', output: output }));
});
gulp.task('zip', cb => {
  const raw = dist + '/raw';
  return gulp
    .src(raw + '/**/*', { base: raw })
    .pipe(zip('application.zip'))
    .pipe(gulp.dest(dist));
});
gulp.task('run', () => {
  const web = spawn('dotnet run', {
    cwd: webName,
    shell: true
  });
  web.stdout.on('data', data => console.log('stdout: ' + data));
  web.stderr.on('data', data => console.log('stdout: ' + data));
  return web;
});
