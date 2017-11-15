import command
import re
import requests

@when(u'you open {url}')
def step_impl(context, url):
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
    context.log.info('opening url {}'.format(url))
    response = requests.get(url)
    response.status_code.should.equal(200)
    context.browser_text = response.text

@then(u'you should see "{text}"')
def step_impl(context, text):
    context.browser_text.should.match(r'.*{}.*'.format(text))
