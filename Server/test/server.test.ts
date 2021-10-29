import { Static, Type } from '@sinclair/typebox'
import { app } from "../src/server"
import { test } from "tap";


export var session_id: any;

test('request the "/battleground/create" route', async t => {
//    t.plan(4)
    const create = app
    const res = await create.inject({
        method: 'POST',
        url: '/battleground/create',
        payload: {"player_id": "test"},
    })
    session_id = JSON.parse(res.body).session_id
    t.equal(JSON.parse(res.body).success, true, 'returns a status code of 200')
})


test('request the "/battleground/join" route', async t => {
 //   t.plan(4)
    const create = app
    const res = await create.inject({
        method: 'POST',
        url: '/battleground/join',
        payload: { "player_id": "test2", "session_id": session_id }
    })
    t.equal(JSON.parse(res.body).success, true, 'returns a status code of 200')
})

test('Invalid request the "/battleground/join" route', async t => {
    t.plan(4)
    const create = app
    const res = await create.inject({
        method: 'POST',
        url: '/battleground/join',
        payload: { "player_id": "test", "session_id": "121" }
    })
    t.equal(JSON.parse(res.body).success, false, 'session not found')
})

test('Duplicate playerId request the "/battleground/join" route', async t => {
    t.plan(4)
    const create = app
    const res = await create.inject({
        method: 'POST',
        url: '/battleground/join',
        payload: { "player_id": "test", "session_id": session_id }
    })
    t.equal(JSON.parse(res.body).success, false, 'Same player id as host')
})


test('failure request the "/battleground/info" route', async t => {
//    t.plan(4)
    const create = app
    const res = await create.inject({
        method: 'POST',
        url: '/battleground/info',
        payload: { "session_id": session_id }
    })
    t.equal(JSON.parse(res.body).success, true, 'returns a status code of 200')
})

test('failure request to the "/battleground/info" route', async t => {
    //    t.plan(4)
    const create = app
    const res = await create.inject({
        method: 'POST',
        url: '/battleground/info',
        payload: { "session_id": "2de" }
    })
    t.equal(JSON.parse(res.body).success, false, 'Session not found')
})

test('request the "/battleground/battle_log" route', async t => {
    //    t.plan(4)
    const create = app
    const res = await create.inject({
        method: 'POST',
        url: '/battleground/battle_log',
        payload: { "from": 0, "to": 1, "session_id": session_id }
    })
    t.equal(JSON.parse(res.body).success, true, 'returns a status code of 200')
})

test('Invalid request to the "/battleground/battle_log" route', async t => {
    //    t.plan(4)
    const create = app
    const res = await create.inject({
        method: 'POST',
        url: '/battleground/battle_log',
        payload: { "from": 0, "to": 1, "session_id": "2db" }
    })
    t.equal(JSON.parse(res.body).success, false, 'Session not found')
})

test('request with neg var "to" the "/battleground/battle_log" route', async t => {
    //    t.plan(4)
    const create = app
    const res = await create.inject({
        method: 'POST',
        url: '/battleground/battle_log',
        payload: { "from": 0, "to": -1, "session_id": session_id }
    })
    t.equal(JSON.parse(res.body).success, true, 'returns a status code of 200')
})

test('request the "/battleground" route', async t => {
    //    t.plan(4)
    const create = app
    const res = await create.inject({
        method: 'POST',
        url: '/battleground',
        payload: {"session_id": session_id, "player_id": "test", pos: {x: 5, y: 10} }
    })
    t.equal(JSON.parse(res.body).success, true, 'returns a status code of 200')
})

test('Invalid request to the "/battleground" route', async t => {
    //    t.plan(4)
    const create = app
    const res = await create.inject({
        method: 'POST',
        url: '/battleground',
        payload: { "session_id": "adb", "player_id": "test", pos: { x: 5, y: 10 } }
    })
    t.equal(JSON.parse(res.body).success, false, 'Session not found')
})

test('request the /battleground/board/upload route', async t => {
    const create = app
    const res = await create.inject({
        method: 'POST',
        url: '/battleground/board/upload',
        payload: { "session_id": session_id, "player_id": "test", pos: { x: 5, y: 10 }}
    })
    t.equal(JSON.parse(res.body).success, true, 'returns a status code of 200')
})

test('Invalid request the /battleground/board/upload route', async t => {
    const create = app
    const res = await create.inject({
        method: 'POST',
        url: '/battleground/board/upload',
        payload: { "session_id": "cde", "player_id": "test", pos: { x: 5, y: 10 } }
    })
    t.equal(JSON.parse(res.body).success, false, 'Session not found')
})


test('request the /battleground/board/sync route', async t => {
    const create = app
    const res = await create.inject({
        method: 'POST',
        url: '/battleground/board/sync',
        payload: { "session_id": session_id }
    })
    t.equal(JSON.parse(res.body).success, true, 'returns a status code of 200')
})

test('Invalid request the /battleground/board/sync route', async t => {
    const create = app
    const res = await create.inject({
        method: 'POST',
        url: '/battleground/board/sync',
        payload: { "session_id": "tgr" }
    })
    t.equal(JSON.parse(res.body).success, false, 'Session not found')
    t.teardown(() => create.close())
})

