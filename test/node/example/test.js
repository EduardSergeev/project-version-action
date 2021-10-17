const assert = require('assert');

describe('Version', function() {
  it('Check that package version is correctly set', function() {
    const expected = process.env.VERSION || '0.0.0';
    const package = require('./package.json');
    assert.equal(package.version, expected);
  });
});
