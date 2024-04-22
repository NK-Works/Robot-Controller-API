--
-- PostgreSQL database dump
--

-- Dumped from database version 16.2
-- Dumped by pg_dump version 16.2

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: map; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.map (
    id integer NOT NULL,
    columns integer NOT NULL,
    rows integer NOT NULL,
    "Name" character varying(50) NOT NULL,
    description character varying(800),
    createddate timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    modifieddate timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    issquare boolean GENERATED ALWAYS AS (((rows > 0) AND (rows = columns))) STORED
);


ALTER TABLE public.map OWNER TO postgres;

--
-- Name: map_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.map ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.map_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: robotcommand; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.robotcommand (
    id integer NOT NULL,
    "Name" character varying(50) NOT NULL,
    description character varying(800),
    ismovecommand boolean NOT NULL,
    createddate timestamp without time zone NOT NULL,
    modifieddate timestamp without time zone NOT NULL
);


ALTER TABLE public.robotcommand OWNER TO postgres;

--
-- Name: robotcommand_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.robotcommand ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.robotcommand_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Data for Name: map; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.map (id, columns, rows, "Name", description, createddate, modifieddate) FROM stdin;
1	10	10	10x10 Map	Test Map	2024-03-02 00:45:22.415557	2024-03-02 00:45:22.415557
2	8	8	8x8 Map	A new map	2024-03-02 00:45:46.302432	2024-03-02 00:45:46.302432
3	20	15	20x15 Map	A non-square map	2024-03-02 00:47:37.936139	2024-03-02 00:47:37.936139
\.


--
-- Data for Name: robotcommand; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.robotcommand (id, "Name", description, ismovecommand, createddate, modifieddate) FROM stdin;
1	LEFT	\N	t	2024-03-01 00:00:00	2024-03-01 00:00:00
2	RIGHT	\N	t	2024-03-01 00:00:00	2024-03-01 00:00:00
3	MOVE	Move the robot forward	t	2024-03-01 00:00:00	2024-03-01 00:00:00
4	PLACE	Place the robot at a specific location	f	2024-03-01 00:00:00	2024-03-01 00:00:00
5	REPORT	Report the current state of the robot	f	2024-03-01 00:00:00	2024-03-01 00:00:00
\.


--
-- Name: map_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.map_id_seq', 4, true);


--
-- Name: robotcommand_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.robotcommand_id_seq', 6, true);


--
-- PostgreSQL database dump complete
--

