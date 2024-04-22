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
    name character varying(50) NOT NULL,
    description character varying(800),
    created_date timestamp without time zone,
    modified_date timestamp without time zone,
    is_square boolean GENERATED ALWAYS AS (((rows > 0) AND (rows = columns))) STORED
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
-- Name: robot_command; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.robot_command (
    id integer NOT NULL,
    name character varying(50) NOT NULL,
    description character varying(800),
    is_move_command boolean NOT NULL,
    created_date timestamp without time zone,
    modified_date timestamp without time zone
);


ALTER TABLE public.robot_command OWNER TO postgres;

--
-- Name: robotcommand_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.robot_command ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.robotcommand_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: user; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."user" (
    id integer NOT NULL,
    email character varying(255) NOT NULL,
    first_name character varying(100) NOT NULL,
    last_name character varying(100) NOT NULL,
    password_hash character varying(255) NOT NULL,
    description text,
    role character varying(50),
    created_date timestamp without time zone,
    modified_date timestamp without time zone
);


ALTER TABLE public."user" OWNER TO postgres;

--
-- Name: user_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.user_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.user_id_seq OWNER TO postgres;

--
-- Name: user_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.user_id_seq OWNED BY public."user".id;


--
-- Name: user id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."user" ALTER COLUMN id SET DEFAULT nextval('public.user_id_seq'::regclass);


--
-- Data for Name: map; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.map (id, columns, rows, name, description, created_date, modified_date) FROM stdin;
1	10	10	10x10 Map	Test Map	2024-03-02 00:45:22.415557	2024-03-02 00:45:22.415557
2	8	8	8x8 Map	A new map	2024-03-02 00:45:46.302432	2024-03-02 00:45:46.302432
3	20	15	20x15 Map	A non-square map	2024-03-02 00:47:37.936139	2024-03-02 00:47:37.936139
16	90	70	string	SwaggerUI Test	2024-03-03 00:00:00	2024-03-03 00:00:00
17	0	110	back	string	2024-03-03 20:32:35.49	2024-03-03 20:32:35.49
\.


--
-- Data for Name: robot_command; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.robot_command (id, name, description, is_move_command, created_date, modified_date) FROM stdin;
1	LEFT	\N	t	2024-03-01 00:00:00	2024-03-01 00:00:00
2	RIGHT	\N	t	2024-03-01 00:00:00	2024-03-01 00:00:00
3	MOVE	Move the robot forward	t	2024-03-01 00:00:00	2024-03-01 00:00:00
4	PLACE	Place the robot at a specific location	f	2024-03-01 00:00:00	2024-03-01 00:00:00
5	REPORT	Report the current state of the robot	f	2024-03-01 00:00:00	2024-03-01 00:00:00
\.


--
-- Data for Name: user; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."user" (id, email, first_name, last_name, password_hash, description, role, created_date, modified_date) FROM stdin;
6	anneshu123@gmail.com	Never	Know	$2a$11$1uJN8ZOxKLAvkReDiH32N.n/4nMAXjvr2lieUII08XMULobd.EKl2	\N	Admin	2024-04-03 14:16:07.629414	2024-04-03 14:16:07.630309
9	anneshu12@gmail.com	Neverkk	Know	$2a$11$m6ft5Pnuy6eu8p5idZNSOeTJlfbxxQ6iBnNP5wNVoa8mVZOZyzER.	\N	Admin	2024-04-06 16:40:12.190104	2024-04-06 16:40:12.190392
10	anneshunag@gmail.com	Anneshu	Nag	$2a$11$4VrLmfUGAG4kKVxyzkQz2uKLR2zE4f9UsLXuiB1dk6t19TGtSNTdW	\N	Admin	2024-04-06 17:59:43.75827	2024-04-06 17:59:43.758442
11	anneshutest@gmail.com	Ann	Nag	$2a$11$0YCbgGQGgeVm6ewe.xIdQeKC5lNHGPAWNAi173CZQyrwRa5DN/bvC	\N	Admin	2024-04-07 16:15:56.537673	2024-04-07 16:15:56.553256
\.


--
-- Name: map_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.map_id_seq', 19, true);


--
-- Name: robotcommand_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.robotcommand_id_seq', 24, true);


--
-- Name: user_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.user_id_seq', 11, true);


--
-- Name: map pk_map; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.map
    ADD CONSTRAINT pk_map PRIMARY KEY (id);


--
-- Name: robot_command pk_robotcommand; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.robot_command
    ADD CONSTRAINT pk_robotcommand PRIMARY KEY (id);


--
-- Name: user user_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."user"
    ADD CONSTRAINT user_pkey PRIMARY KEY (id);


--
-- PostgreSQL database dump complete
--

