import command
import re
import requests

@when(u'you get {url}')
def step_impl(context, url):
    url = resolve_url(context, url)
    context.log.info('getting url {}'.format(url))
    response = requests.get(url)
    response.status_code.should.equal(200)
    context.browser_text = response.text

@when(u'you post "{data}" to {url}')
def step_impl(context, data, url):
    url = resolve_url(context, url)
    fields = data.split('=')
    assert len(fields) == 2, 'Invalid data format: {}'.format(data)
    form = { fields[0]: fields[1] }
    context.log.info('posting url {} {}'.format(url, form))
    response = requests.post(url, data=form)
    response.status_code.should.equal(200)

@then(u'you should see "{text}"')
def step_impl(context, text):
    context.browser_text.should.match(r'.*{}.*'.format(text))

def resolve_url(context, url):
    pseudo = re.match('https?://(([^.]+).x.y.z)/', url)
    if pseudo:
        hostname = pseudo.group(1)
        appname = pseudo.group(2)
        cmd = command.Command(context, 'cf app {}'.format(appname))
        cmd.run()
        route = re.search(r'^routes:\s+(.*)', cmd.stdout, re.MULTILINE)
        assert route, 'could not determine app route'
        real_hostname = route.group(1)
        url = url.replace(hostname, real_hostname)
    return url
