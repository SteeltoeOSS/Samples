import mechanicalsoup
import resolve
import time

@when(u'you get {url}')
def step_impl(context, url):
    url = resolve.url(context, url)
    context.log.info('getting url {}'.format(url))
    context.browser = mechanicalsoup.StatefulBrowser()
    attempt = 0
    while True:
        attempt += 1
        resp = context.browser.open(url)
        if resp.status_code == 200:
            context.log.info('opened {}'.format(url))
            break
        context.log.info('failed to open {} [{}]'.format(url, resp.status_code))
        if resp.status_code >= 500 and attempt <= 5:
            time.sleep(1)
            continue
        raise Exception('Unable to open page {} [{}]'.format(url, resp.status_code))

@when(u'you post "{data}" to {url}')
def step_impl(context, data, url):
    url = resolve.url(context, url)
    fields = data.split('=')
    assert len(fields) == 2, 'Invalid data format: {}'.format(data)
    payload = { fields[0]: fields[1] }
    context.log.info('posting url {} {}'.format(url, payload))
    context.browser = mechanicalsoup.StatefulBrowser()
    context.browser.post(url, data=payload).status_code.should.equal(200)

@when(u'you login with "{username}"/"{password}"')
def step_impl(context, username, password):
    context.browser.select_form('form[action="/login.do"]')
    context.browser['username'] = username
    context.browser['password'] = password
    context.browser.submit_selected().status_code.should.equal(200)

@then(u'you should be at {url}')
def step_impl(context, url):
    context.browser.get_url().should.match(r'{}(\?.*)?'.format(resolve.url(context, url)))

@then(u'you should see "{text}"')
def step_impl(context, text):
    context.browser.get_current_page().get_text().should.match(r'.*{}.*'.format(text))
