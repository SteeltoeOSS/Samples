import time

import mechanicalsoup
import requests
from behave import *

from pysteel import command, dns


@when(u'you get {url}')
def step_impl(context, url):
    """
    :type context: behave.runner.Context
    :type url: str
    """
    url = dns.resolve_url(context, url)
    context.log.info('getting url {}'.format(url))
    context.browser = mechanicalsoup.StatefulBrowser()
    attempt = 0
    while True:
        attempt += 1
        resp = context.browser.open(url)
        if resp.status_code < 500:
            context.log.info('GET {} [{}]'.format(url, resp.status_code))
            resp.status_code.should.equal(200)
            if context.browser.page is None:
                context.response_text = resp.text
            break
        context.log.info('failed to get {} [{}]'.format(url, resp.status_code))
        if attempt > 5:
            raise Exception('Unable to get page {} [{}]'.format(url, resp.status_code))
        time.sleep(context.options.cmd.loop_wait)


@when(u'you post "{data}" to {url}')
def step_impl(context, data, url):
    """
    :type context: behave.runner.Context
    :type data: str
    :type url: str
    """
    url = dns.resolve_url(context, url)
    fields = data.split('=')
    assert len(fields) == 2, 'Invalid data format: {}'.format(data)
    payload = {fields[0]: fields[1]}
    context.log.info('posting url {} {}'.format(url, payload))
    context.browser = mechanicalsoup.StatefulBrowser()
    resp = context.browser.post(url, data=payload)
    context.log.info('POST {} [{}]'.format(url, resp.status_code))


@when(u'you login with "{username}"/"{password}"')
def step_impl(context, username, password):
    """
    :type context: behave.runner.Context
    :type username: str
    :type password: str
    """
    context.browser.select_form('form[action="/login.do"]')
    context.browser['username'] = username
    context.browser['password'] = password
    context.browser.submit_selected()


@then(u'you should be at {url}')
def step_impl(context, url):
    """
    :type context: behave.runner.Context
    :type url: str
    """
    context.browser.get_url().should.match(r'{}(\?.*)?'.format(dns.resolve_url(context, url)))


@then(u'you should see "{text}"')
def step_impl(context, text):
    """
    :type context: behave.runner.Context
    :type text: str
    """
    context.browser.page.get_text().should.match(r'.*{}.*'.format(text))


@then(u'the response should contain "{text}"')
def step_impl(context, text):
    """
    :type context: behave.runner.Context
    :type text: str
    """
    context.response_text.should.match(r'.*{}.*'.format(text))

@then(u'you should be able to access CloudFoundry app {app} management endpoints')
def step_impl(context, app):
    """
    :type context: behave.runner.Context
    :type app: str
    """
    url = dns.resolve_url(context, 'https://{}/cloudfoundryapplication'.format(app))
    token = get_oauth_token(context)
    resp = requests.get(url, headers={'Authorization': token})
    resp.status_code.should.equal(200)
    # context.log.info(resp.content)
    for endpoint in ['beans', 'dbmigrations', 'env', 'health', 'heapdump', 'httpexchanges', 'info', 'loggers', 'mappings', 'prometheus', 'refresh', 'threaddump']:
        resp.text.should.contain('/cloudfoundryapplication/{}'.format(endpoint))

@then(u'CloudFoundry app {app} should be healthy')
def step_impl(context, app):
    """
    :type context: behave.runner.Context
    :type app: str
    """
    url = dns.resolve_url(context, 'https://{}/cloudfoundryapplication/health'.format(app))
    token = get_oauth_token(context)
    attempt = 0
    while True:
        attempt += 1
        resp = requests.get(url, headers={'Authorization': token})
        if resp.status_code < 500:
            context.log.info('GET {} [{}]'.format(url, resp.status_code))
            resp.status_code.should.equal(200)
            resp.text.should.equal('{"status":"UP"}')
            break
        context.log.info('failed to get {} [{}]'.format(url, resp.status_code))
        if attempt > 5:
            raise Exception('Unable to get page {} [{}]'.format(url, resp.status_code))
        time.sleep(context.options.cmd.loop_wait)


def get_oauth_token(context):
    """
    :type context: behave.runner.Context
    """
    cmd = command.Command(context, 'cf oauth-token')
    cmd.run()
    return cmd.stdout.strip()
