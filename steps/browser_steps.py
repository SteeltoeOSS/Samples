import command
import re
import requests

@when(u'you open {url}')
def step_impl(context, url):
    if 'foo.x.y.z' in url:
        cmd = command.Command(context, 'cf app foo')
        cmd.run()
        match = re.search(r'^routes:\s+(.*)', cmd.stdout, re.MULTILINE)
        assert match, 'could not determine app route'
        hostname = match.group(1)
        url = url.replace('foo.x.y.z', hostname)
    response = requests.get(url)
    response.status_code.should.equal(200)
    context.browser_text = response.text

@then(u'you should see "{text}"')
def step_impl(context, text):
    context.browser_text.should.match(r'.*{}.*'.format(text))
